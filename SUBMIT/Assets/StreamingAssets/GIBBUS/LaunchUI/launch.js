"use strict";
(() => {
  const $ = id => document.getElementById(id);
  const el = {
    connection: $("connection-label"), phase: $("phase-label"), checklist: $("checklist"),
    counter: $("check-counter"), progress: $("check-progress"), readiness: $("readiness-percent"),
    ring: $("readiness-ring"), launchState: $("launch-state"), launchMessage: $("launch-message"),
    authorize: $("authorize-button"), abort: $("abort-button"), note: $("authorization-note"),
    altitude: $("altitude"), velocity: $("velocity"), fuel: $("fuel"), power: $("power"),
    altitudeBar: $("altitude-bar"), velocityBar: $("velocity-bar"), fuelBar: $("fuel-bar"),
    powerBar: $("power-bar"), comms: $("comms"), trajectory: $("trajectory"),
    countdown: $("countdown"), directorStatus: $("director-status"), directorIndicator: $("director-indicator"),
    launchControlStatus: $("launch-control-status"), launchIndicator: $("launch-indicator"),
    missionCode: $("mission-code"), objective: $("mission-objective"), log: $("event-log"),
    toast: $("toast"), confirm: $("confirm-overlay"), confirmYes: $("confirm-yes"),
    confirmCancel: $("confirm-cancel"), clock: $("clock")
  };
  let snapshot = null, connected = false, pending = false, toastTimer = null;
  const seenLog = new Set();

  function send(message) {
    if (window.ceffy && typeof window.ceffy.SendToUnity === "function") window.ceffy.SendToUnity(message);
  }
  function notify(message) {
    el.toast.textContent = message;
    el.toast.hidden = false;
    clearTimeout(toastTimer);
    toastTimer = setTimeout(() => el.toast.hidden = true, 3500);
  }
  function addLog(message, timestamp) {
    const key = `${timestamp}|${message}`;
    if (seenLog.has(key)) return;
    seenLog.add(key);
    const p = document.createElement("p");
    const time = document.createElement("time");
    time.textContent = timestamp || new Date().toLocaleTimeString([], {hour:"2-digit",minute:"2-digit",hour12:false});
    p.append(time, document.createTextNode(message));
    el.log.prepend(p);
    while (el.log.children.length > 4) el.log.lastElementChild.remove();
  }
  function setMetric(node, value, suffix) {
    node.innerHTML = `${value}<small> ${suffix}</small>`;
  }
  function setBar(node, value) {
    node.style.width = `${Math.max(0, Math.min(100, Number(value) || 0))}%`;
  }
  function renderChecklist(items) {
    el.checklist.replaceChildren();
    (items || []).forEach(item => {
      const row = document.createElement("button");
      row.className = `check-item${item.acknowledged ? " checked" : ""}`;
      row.type = "button";
      row.disabled = pending || snapshot.launchAuthorized;
      row.innerHTML = `<span class="check-box">${item.acknowledged ? "✓" : ""}</span>
        <span class="check-copy"><strong></strong><small></small></span>
        <span class="check-state">${item.acknowledged ? "ACK" : "REVIEW"}</span>`;
      row.querySelector(".check-copy strong").textContent = item.title;
      row.querySelector(".check-copy small").textContent = item.description;
      row.addEventListener("click", () => runCommand("setChecklistItem", item.id, !item.acknowledged));
      el.checklist.append(row);
    });
  }
  function render(data) {
    snapshot = data;
    connected = true;
    el.connection.textContent = "UNITY LINK ESTABLISHED";
    el.connection.previousElementSibling.style.background = "var(--green)";
    el.phase.textContent = data.missionPhase || "PRE-LAUNCH";
    el.missionCode.textContent = data.missionCode || "GIBBUS-01";
    el.objective.textContent = data.missionObjective || "";
    renderChecklist(data.checklist);

    const total = (data.checklist || []).length;
    const complete = (data.checklist || []).filter(x => x.acknowledged).length;
    const percent = total ? Math.round(complete / total * 100) : 0;
    el.counter.textContent = `${complete} / ${total}`;
    el.progress.style.width = `${percent}%`;
    el.readiness.textContent = `${percent}%`;
    el.ring.style.borderColor = percent === 100 ? "var(--green)" : "var(--line-strong)";

    if (data.launchAuthorized) {
      el.launchState.textContent = "LAUNCH AUTHORISED";
      el.launchMessage.textContent = "Authorisation accepted by Unity. Awaiting the launch-sequence controller.";
      el.authorize.disabled = true;
      el.authorize.textContent = "AUTHORISATION ACCEPTED";
      el.note.textContent = "The game controls the countdown and launch animation.";
      el.directorStatus.textContent = "Checklist acknowledged";
      el.directorIndicator.textContent = "GO";
      el.directorIndicator.className = "status-text green-text";
      el.launchControlStatus.textContent = "Sequence hand-off requested";
      el.launchIndicator.textContent = "AUTHORISED";
      el.launchIndicator.className = "status-text green-text";
    } else {
      el.launchState.textContent = percent === 100 ? "READY FOR REVIEW" : "SYSTEMS REVIEW";
      el.launchMessage.textContent = percent === 100
        ? "All checklist items are acknowledged. The flight director may request launch authorisation."
        : "Complete the pre-launch review. Each acknowledgement is recorded by the mission interface.";
      el.authorize.disabled = percent !== 100 || pending;
      el.authorize.innerHTML = "AUTHORISE LAUNCH <span>↗</span>";
      el.note.textContent = percent === 100
        ? "All acknowledgements complete. Authorisation still requires confirmation."
        : "Launch authorisation remains locked until all checklist items are acknowledged.";
      el.directorStatus.textContent = `${complete} of ${total} checklist items acknowledged`;
      el.directorIndicator.textContent = percent === 100 ? "READY" : "STANDBY";
      el.directorIndicator.className = `status-text ${percent === 100 ? "green-text" : "amber-text"}`;
      el.launchControlStatus.textContent = "Awaiting authorisation";
      el.launchIndicator.textContent = "HOLD";
      el.launchIndicator.className = "status-text amber-text";
    }

    const t = data.telemetry || {};
    setMetric(el.altitude, Number(t.altitudeKm || 0).toFixed(1), "km");
    setMetric(el.velocity, Number(t.velocityKmPerSecond || 0).toFixed(2), "km/s");
    setMetric(el.fuel, Math.round(t.propellantPercent ?? 100), "%");
    setMetric(el.power, Math.round(t.powerPercent ?? 100), "%");
    setBar(el.altitudeBar, Math.min(100, Number(t.altitudeKm || 0) / 400 * 100));
    setBar(el.velocityBar, Math.min(100, Number(t.velocityKmPerSecond || 0) / 11 * 100));
    setBar(el.fuelBar, t.propellantPercent ?? 100);
    setBar(el.powerBar, t.powerPercent ?? 100);
    el.comms.textContent = t.communicationsStatus || "STANDBY";
    el.trajectory.textContent = t.trajectory || "EARTH → LUNAR TRANSFER";
    el.countdown.textContent = data.countdownSeconds >= 0
      ? `T − 00:${String(data.countdownSeconds).padStart(2,"0")}` : "T − --:--";

    (data.eventLog || []).forEach(entry => addLog(entry.message, entry.timestamp));
    if (data.launchAuthorized && !seenLog.has("launch-authorised")) {
      seenLog.add("launch-authorised");
      addLog("Launch authorisation accepted by Unity.");
    }
    el.abort.disabled = pending;
  }

  async function runCommand(method, ...args) {
    if (!connected || pending || !window.ceffy?.unity?.[method]) return;
    pending = true;
    if (snapshot) render(snapshot);
    try {
      const result = await window.ceffy.unity[method](...args);
      if (method === "authorizeLaunch" && result === false) notify("Authorisation rejected. Review the checklist.");
      const fresh = await window.ceffy.unity.getSnapshot();
      render(fresh);
    } catch (error) {
      console.error("[GIBBUS UI]", error);
      notify(`Unity command failed: ${error.message || error}`);
    } finally {
      pending = false;
      if (snapshot) render(snapshot);
    }
  }

  function registerUiBridge() {
    if (!window.ceffy || !window.ceffy.ui) return;
    window.ceffy.ui.updateSnapshot = render;
    window.ceffy.ui.showNotice = notify;
  }

  // First handshake asks Unity to inject the Ceffy bridge runtime.
  const pageHandshake = setInterval(() => {
    if (window.ceffy?.SendToUnity) {
      send("gibbus:page-loaded");
      clearInterval(pageHandshake);
    }
  }, 100);

  document.addEventListener("ceffy:ready", () => {
    registerUiBridge();
    const bridgeHandshake = setInterval(() => {
      if (window.ceffy?.unity?.getSnapshot) {
        registerUiBridge();
        send("gibbus:bridge-ready");
        clearInterval(bridgeHandshake);
      }
    }, 100);
  });

  el.authorize.addEventListener("click", () => {
    if (!snapshot || snapshot.launchAuthorized || el.authorize.disabled) return;
    el.confirm.hidden = false;
  });
  el.confirmCancel.addEventListener("click", () => el.confirm.hidden = true);
  el.confirmYes.addEventListener("click", () => {
    el.confirm.hidden = true;
    runCommand("authorizeLaunch");
  });
  el.abort.addEventListener("click", () => runCommand("requestReturnToDesign"));
  window.addEventListener("keydown", event => {
    if (event.key === "Escape" && !el.confirm.hidden) el.confirm.hidden = true;
  });
  setInterval(() => {
    el.clock.textContent = new Date().toLocaleTimeString([], {hour:"2-digit",minute:"2-digit",second:"2-digit",hour12:false});
  }, 1000);
  addLog("Launch operations interface initialising.");

  const stageStatus = document.getElementById("stage-status");

  document.querySelectorAll(".stage-button").forEach(button => {
    button.addEventListener("click", () => {
      const stageId = button.dataset.stage;

      if (!window.ceffy || !window.ceffy.unity ||
          typeof window.ceffy.unity.RequestStageSeparation !== "function") {
        if (stageStatus) stageStatus.textContent = "UNITY LINK UNAVAILABLE";
        return;
      }

      const accepted = window.ceffy.unity.RequestStageSeparation(stageId);

      // Ceffy bridge calls are synchronous for this command.
      if (accepted) {
        button.disabled = true;
        button.textContent = "SEPARATION COMMANDED";
        if (stageStatus) stageStatus.textContent = `${stageId.toUpperCase()} separation accepted.`;
      } else {
        if (stageStatus) stageStatus.textContent = "COMMAND REJECTED: CHECK STAGE ORDER.";
      }
    });
  });
})();