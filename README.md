# 🚀 GIBBUS
### Design. Engineer. Explore.

**An interactive space mission design and simulation experience.**

GIBBUS is an educational space exploration game developed by **JETSTACK** for the 2026 NASA International Space Apps Challenge, under the *Space Mission Design Game* challenge.

The project aims to transform the complex process of designing a space mission into an interactive experience where players take responsibility for engineering decisions, scientific objectives, spacecraft configuration, resource management, and mission execution.

Rather than simply teaching players about space exploration, GIBBUS allows them to experience the decision-making processes behind it.

---

# 1. Project Overview

Designing a successful space mission requires much more than building a spacecraft and launching it.

Every decision introduces constraints, compromises, and consequences. Increasing scientific payload mass affects spacecraft requirements. Selecting a launch vehicle influences mission capacity and budget. Power systems, propulsion, communications, orbital mechanics, and safety requirements must all work together.

GIBBUS brings these interconnected engineering challenges into one interactive environment.

Players take on the role of a mission designer, working through different stages of mission development to create a viable space expedition.

The initial campaign, **GIBBUS-01**, focuses on a lunar science expedition.

The objective is to design a mission capable of travelling from Earth to the Moon, conducting scientific operations, and safely returning to Earth.

# 2. Project Objectives

GIBBUS aims to:

- Make aerospace engineering more accessible through interactive gameplay.
- Introduce players to real-world spacecraft and mission design principles.
- Demonstrate the relationship between engineering decisions and mission success.
- Encourage scientific curiosity and critical thinking.
- Incorporate authentic NASA datasets and scientific information.
- Provide an environment for experimenting with different mission architectures.
- Demonstrate how mathematics, physics, computer science, and engineering work together in space exploration.

# 3. Gameplay Concept

The experience follows the development and execution of a space mission.

Players progress through a structured mission-design process, making decisions that influence the feasibility and performance of their expedition.

## Mission Design Workflow

The intended mission-design workflow consists of nine stages:

1. **Mission Objectives** – Define the purpose, scientific goals, and requirements of the expedition.
2. **Launch Vehicle Selection** – Select a launch system capable of delivering the mission payload.
3. **Spacecraft Architecture** – Configure the spacecraft and determine how its components work together.
4. **Scientific Payload** – Select scientific instruments and experiments.
5. **Resource Management** – Balance mass, power, propulsion, and financial constraints.
6. **Communications** – Establish communication requirements between the spacecraft and mission control.
7. **Trajectory Design** – Consider orbital requirements and the spacecraft's journey.
8. **Contingency Planning** – Account for potential mission complications and operational risks.
9. **Mission Validation** – Evaluate the completed design and determine whether it is ready for execution.

Each stage contributes to the overall mission architecture.

## Mission Execution

Following mission validation, the intended experience progresses into mission execution:

- Launch preparation and liftoff.
- Atmospheric ascent.
- Orbital insertion.
- Translunar journey.
- Lunar approach and landing.
- Scientific operations.
- Lunar ascent and return journey.
- Earth re-entry and mission completion.
- Final mission evaluation.

The player's engineering decisions influence the challenges and outcomes encountered throughout the mission.

# 4. Engineering Systems

GIBBUS focuses on interconnected engineering systems rather than treating spacecraft components as isolated selections.

| System | Purpose |
|---|---|
| Launch Systems | Determine launch capabilities and payload capacity. |
| Spacecraft Architecture | Define spacecraft configuration and modules. |
| Scientific Payload | Provide instruments required for mission objectives. |
| Mass Management | Ensure the spacecraft remains within mass constraints. |
| Power Management | Account for electrical power requirements. |
| Propulsion | Consider propulsion capabilities and fuel requirements. |
| Communications | Establish spacecraft communication capabilities. |
| Orbital Mechanics | Introduce orbital and trajectory considerations. |
| Mission Budget | Manage financial limitations. |
| Mission Safety | Evaluate risks and mission reliability. |

These systems interact.

For example, selecting additional scientific instruments may improve the scientific value of a mission while increasing its mass and power requirements.

This creates meaningful engineering trade-offs.

# 5. GIBBUS-01: Lunar Expedition

GIBBUS-01 serves as the initial campaign and primary demonstration mission.

It focuses on a crewed expedition to the Moon, incorporating spacecraft configuration, lunar exploration, scientific research, and Earth return operations.

The mission architecture is based on three principal spacecraft elements:

- **Crew Return Spacecraft:** Provides the crew with a habitable vehicle for the journey and return.
- **Service Module:** Supports essential spacecraft operations and propulsion requirements.
- **Lunar Lander:** Enables the crew to travel between lunar orbit and the lunar surface.

The player must ensure that the selected architecture can support the intended expedition.

The campaign provides a practical context for exploring how spacecraft systems interact during a real-world-inspired mission.

# 6. NASA Data Integration

## 6.1 Overview

One of the defining technical aspects of GIBBUS is its integration of publicly available scientific information and planetary datasets.

Rather than relying entirely on fictional environments and arbitrary values, GIBBUS aims to incorporate information derived from actual space research and NASA missions.

These datasets provide scientific context for mission planning and can support interactive gameplay systems.

The primary data categories include:

- Lunar surface imagery and mapping.
- Lunar surface elevation and topography.
- Landing-site illumination.
- Planetary and lunar orbital positions.
- Earth satellite imagery.
- Radiation measurements.
- Scientific mission archives.

The following services form the project's NASA and planetary-data integration reference.

---

## 6.2 Moon Surface Imagery and Mapping

### A. NASA Moon Trek – WMTS

<text>**Service:** NASA Moon Trek</text>

<text>**Protocol:** Web Map Tile Service (WMTS)</text>

Moon Trek provides access to lunar imagery through map layers and tiled imagery services.

It is particularly useful for the interactive spherical Moon model in GIBBUS.

**Direct imagery endpoint:**

```text
https://moontrek.jpl.nasa.gov/trektiles/Moon/EQ/LRO_WAC_Mosaic_Global_303ppd_v02/1.0.0/default/default028mm/0/0/0.jpg
```

**GetCapabilities / available map layers:**

[Moon Trek WMTS API documentation](https://trek.nasa.gov/tiles/apidoc/trekAPI.html?body=moon)

**Example lunar imagery tile:**

[View lunar imagery tile](https://trek.nasa.gov/tiles/Moon/EQ/LRO_WAC_Mosaic_Global_303ppd_v02/1.0.0/default/default028mm/0/0/0.jpg)

**Data provided:**
- Lunar surface imagery.
- Global Moon mosaics.
- Image tiles for mapping.
- Multiple zoom levels.
- Different lunar map layers.

**Application in GIBBUS:**

Moon Trek imagery can be used to construct a detailed lunar globe inside Unity.

Instead of relying on a single static image, the application can retrieve map tiles at different zoom levels, allowing players to inspect specific lunar regions.

This is particularly useful when investigating potential landing destinations.

### B. NASA Planetary Data System (PDS) – Search API

<text>**Service:** NASA Planetary Data System</text>

<text>**Protocol:** REST API</text>

The Planetary Data System is NASA's archive for planetary science data.

It provides access to scientific products, mission archives, metadata, and references to planetary datasets.

**Base endpoint:**

```text
https://pds.nasa.gov/api/search/1/
```

**Search for lunar scientific products:**

```text
https://pds.nasa.gov/api/search/1/products?limit=10&q=ref_lid_target%20eq%20%22urn%3Anasa%3Apds%3Acontext%3Atarget%3Asatellite.301%22
```

**General test request:**

```text
https://pds.nasa.gov/api/search/1/products?limit=10
```

**Documentation:**

[PDS Search API documentation](https://nasa-pds.github.io/pds-api/guides/search/endpoints.html)

**Data provided:**
- Scientific product metadata.
- Planetary mission archives.
- Dataset references.
- Scientific observations.
- Links to downloadable data products.

**Application in GIBBUS:**

PDS can act as a scientific data discovery layer.

It allows the project to identify relevant lunar datasets and retrieve references to the actual scientific files.

An important distinction is that searching the archive does not necessarily retrieve the underlying imagery or measurements. Those products may need to be downloaded and processed separately.

---

## 6.3 Lunar Elevation and Landing-Site Data

### C. NASA LOLA – Lunar Orbiter Laser Altimeter

<text>**Mission:** Lunar Reconnaissance Orbiter (LRO)</text>

<text>**Instrument:** Lunar Orbiter Laser Altimeter (LOLA)</text>

<text>**Data service:** PDS Geosciences Node Orbital Data Explorer (ODE)</text>

LOLA is a laser altimeter instrument aboard NASA's Lunar Reconnaissance Orbiter.

Its measurements provide information about the Moon's surface elevation and topography.

This is particularly important for evaluating potential landing destinations.

**Base endpoint:**

```text
https://oderest.rsl.wustl.edu/live2/
```

**Example REST request:**

```text
https://oderest.rsl.wustl.edu/live2/?query=products&target=moon&ihid=LRO&iid=LOLA&pt=GDRDEM&output=json
```

**Parameters:**

| Parameter | Value | Purpose |
|---|---|---|
| query | products | Searches available products. |
| target | moon | Specifies the Moon. |
| ihid | LRO | Identifies the mission. |
| iid | LOLA | Identifies the instrument. |
| pt | GDRDEM | Requests gridded digital elevation products. |
| output | json | Requests JSON output. |

**Data provided:**
- Lunar elevation products.
- Digital elevation model references.
- Product metadata.
- Dataset download references.

**Application in GIBBUS:**

LOLA elevation data can support a more realistic lunar terrain representation.

A processed elevation dataset can be converted into a Unity-compatible heightmap and applied to a spherical terrain mesh.

This creates the possibility of representing actual lunar surface elevation rather than displaying a purely flat texture.

The resulting terrain information can help players evaluate landing-site characteristics such as:
- Surface elevation.
- Local terrain variation.
- Slope and terrain suitability.
- Potential landing hazards.

The original scientific dataset must be processed into an appropriate format before being used as a Unity terrain input.

### D. NASA Moon Trek – South Pole Illumination

<text>**Service:** NASA Moon Trek</text>

<text>**Dataset:** WAC South Pole Illumination</text>

Lunar illumination is an important consideration when planning missions to the lunar south pole.

Sunlight availability affects surface operations, thermal conditions, and the potential generation of solar power.

**Direct WMTS capabilities endpoint:**

```text
https://trek.nasa.gov/tiles/Moon/SP/WAC_POLE_ILL_PCT_SOUTH_100M/1.0.0/WMTSCapabilities.xml
```

**Dataset layer:**

```text
WAC_POLE_ILL_PCT_SOUTH_100M
```

**Related dataset:**

```text
AVGVISIB_85S_060M_201608_EARTH_SP
```

This related layer represents average Earth visibility near the lunar south pole.

**Data provided:**
- Lunar south-pole illumination information.
- Percentage-based illumination mapping.
- Surface sunlight availability.
- Earth visibility information for selected regions.

**Application in GIBBUS:**

Illumination data can contribute to landing-site evaluation.

For example, a region with greater sunlight availability may offer advantages for a mission relying on solar power.

Earth visibility data can also help evaluate communication opportunities with Earth, although actual communications availability additionally depends on spacecraft position, antenna characteristics, and relay infrastructure.

This creates another engineering trade-off when choosing a lunar landing location.

---

## 6.4 Orbital Positions and Trajectories

### E. NASA JPL Horizons API

<text>**Service:** NASA Jet Propulsion Laboratory Horizons</text>

<text>**Protocol:** REST API</text>

JPL Horizons provides access to accurate astronomical and planetary ephemerides.

It can calculate the positions and velocities of celestial bodies over specified time intervals.

**Direct API endpoint:**

```text
https://ssd-api.jpl.nasa.gov/api/horizons.api
```

**Example request – Moon position relative to Earth:**

```text
https://ssd-api.jpl.nasa.gov/api/horizons.api?format=json&COMMAND='301'&OBJ_DATA='YES'&MAKE_EPHEM='YES'&EPHEM_TYPE='VECTORS'&CENTER='500@399'&START_TIME='2026-11-15'&STOP_TIME='2026-11-16'&STEP_SIZE='1%20h'&OUT_UNITS='KM-S'&VEC_TABLE='2'
```

**Documentation:**

[JPL Horizons API parameter reference](https://ssd-api.jpl.nasa.gov/doc/horizons.html)

**Data provided:**
- Celestial position vectors.
- Velocity vectors.
- Time-dependent astronomical positions.
- Orbital information.
- Ephemeris data.

**Application in GIBBUS:**

Horizons can supply real astronomical position data for the Moon and other celestial bodies.

This information can be used to support:
- Earth-Moon positioning.
- Mission departure planning.
- Orbital visualisations.
- Celestial body movement.
- Trajectory planning inputs.

For example, GIBBUS can use the Moon's position relative to Earth to provide more realistic orbital visualisations.

The retrieved data must be converted into the appropriate Unity coordinate system and reference frame.

A further distinction is that ephemeris data provides celestial positions and velocities; it does not automatically generate a complete spacecraft trajectory or validate fuel requirements. Those calculations require additional mission dynamics and propulsion models.

---

## 6.5 Earth Imagery and Satellite Observation

### F. NASA Global Imagery Browse Services (GIBS)

<text>**Service:** NASA Global Imagery Browse Services</text>

<text>**Protocol:** WMTS</text>

NASA GIBS provides access to global satellite imagery and Earth observation visualisations.

It allows applications to retrieve imagery from different satellite instruments and observation dates.

**Base endpoint:**

```text
https://gibs.earthdata.nasa.gov/wmts/epsg4326/best/
```

**GetCapabilities endpoint:**

```text
https://gibs.earthdata.nasa.gov/wmts/epsg4326/best/wmts.cgi?SERVICE=WMTS&REQUEST=GetCapabilities&VERSION=1.0.0
```

**Example MODIS Terra imagery tile:**

```text
https://gibs.earthdata.nasa.gov/wmts/epsg4326/best/MODIS_Terra_CorrectedReflectance_TrueColor/default/2026-11-14/250m/3/2/3.jpg
```

**Documentation:**

[NASA GIBS API documentation](https://nasa-gibs.github.io/gibs-api-docs/access-basics/)

**Data provided:**
- Satellite-derived Earth imagery.
- Atmospheric visualisations.
- Environmental observations.
- Cloud and surface observations.
- Time-specific imagery layers.

**Application in GIBBUS:**

GIBS can provide Earth imagery for the game's planetary visualisation systems.

It can be used to display satellite-derived Earth imagery and potentially illustrate environmental conditions during different stages of the mission.

This is particularly relevant to visualising Earth during orbital operations and atmospheric return sequences.

### G. NASA Satellite Situation Center (SSCWeb)

<text>**Service:** NASA Satellite Situation Center</text>

<text>**Platform:** SSCWeb</text>

SSCWeb provides tools and information relating to spacecraft locations and scientific observations in the context of Earth's space environment.

**Official website:**

[NASA Satellite Situation Center](https://sscweb.gsfc.nasa.gov/)

**Application in GIBBUS:**

SSCWeb may be useful when researching spacecraft positions and geophysical context for scientific satellite observations.

It is important to distinguish this service from a general-purpose live satellite tracker.

It should not be treated as a universal API for retrieving the current position of every Earth-orbiting satellite.

For other satellite-tracking requirements, additional orbital data sources may be necessary.

---

## 6.6 Radiation Measurements

### H. NASA LRO CRaTER

<text>**Mission:** Lunar Reconnaissance Orbiter</text>

<text>**Instrument:** Cosmic Ray Telescope for the Effects of Radiation (CRaTER)</text>

CRaTER is a scientific instrument aboard the Lunar Reconnaissance Orbiter.

It measures radiation in the lunar environment and helps scientists understand the radiation exposure associated with space exploration.

**Data portal:**

[CRaTER Data Products](https://crater-web.sr.unh.edu/)

**NASA archive:**

[Planetary Data System](https://pds.nasa.gov/)

**Data provided:**
- Radiation dose rates.
- Dose equivalent measurements.
- Cosmic-ray measurements.
- Lunar radiation environment observations.

**Application in GIBBUS:**

Radiation information can contribute to mission safety and crew exposure considerations.

It provides an opportunity to introduce radiation as an engineering constraint when designing a crewed lunar expedition.

For example, players could evaluate the importance of radiation protection and spacecraft shielding.

CRaTER is a scientific data archive rather than a conventional live REST API. Its products may need to be downloaded, interpreted, and processed before being incorporated into the game.

---

# 7. NASA API and Data Source Reference

The following table summarises the principal services identified for GIBBUS.

| Service | Direct endpoint | Primary application |
|---|---|---|
| Moon Trek WMTS | [Moon Trek](https://trek.nasa.gov/tiles/apidoc/trekAPI.html?body=moon) | Lunar surface imagery |
| PDS Search API | [PDS API](https://pds.nasa.gov/api/search/1/) | Scientific datasets |
| LOLA ODE | [ODE REST](https://oderest.rsl.wustl.edu/live2/) | Lunar elevation |
| Moon Trek Illumination | [WMTS Layer](https://trek.nasa.gov/tiles/Moon/SP/WAC_POLE_ILL_PCT_SOUTH_100M/1.0.0/WMTSCapabilities.xml) | Landing-site sunlight |
| JPL Horizons | [Horizons API](https://ssd-api.jpl.nasa.gov/api/horizons.api) | Orbital positions |
| NASA GIBS | [GIBS WMTS](https://gibs.earthdata.nasa.gov/wmts/epsg4326/best/wmts.cgi) | Earth imagery |
| NASA SSCWeb | [SSCWeb](https://sscweb.gsfc.nasa.gov/) | Spacecraft location context |
| LRO CRaTER | [CRaTER Portal](https://crater-web.sr.unh.edu/) | Radiation measurements |

**Technical note:** These services have different delivery mechanisms. Some expose REST endpoints, others use WMTS, and some distribute downloadable scientific archive products. They should not all be treated as interchangeable live APIs.

# 8. Proposed Scientific Data Integration Architecture

To make these services usable within Unity, GIBBUS can use a modular data architecture.

The general data flow is:

```text
        NASA DATA SOURCES
                |
                |
    +-----------+-----------+
    |           |           |
 Moon Trek    JPL        PDS / ODE
    |        Horizons       |
    |           |           |
    +-----------+-----------+
                |
                |
       DATA RETRIEVAL LAYER
                |
                |
       DATA PROCESSING LAYER
                |
                |
        UNITY DATA SYSTEMS
                |
       +--------+--------+
       |        |        |
     Moon    Mission   Landing
     Globe   Planning   Analysis
       |        |        |
       +--------+--------+
                |
                |
        PLAYER EXPERIENCE
```

## 8.1 Data Retrieval

Individual services are accessed through dedicated integration components.

Examples include:

- `MoonTrekService`
- `LOLADataService`
- `HorizonsService`
- `GIBSService`
- `PDSDataService`
- `CRaTERDataService`

Each component is responsible for retrieving information from its respective source.

## 8.2 Data Processing

Raw scientific information is not always immediately usable by Unity.

Depending on the source, processing may involve:

- JSON deserialization.
- Image tile retrieval.
- Heightmap conversion.
- Coordinate transformation.
- Unit conversion.
- Data interpolation.
- Caching and local storage.

## 8.3 Unity Integration

Processed information can then be supplied to the relevant game systems.

For example:

| Unity System | Relevant Data |
|---|---|
| Lunar Globe | Moon Trek imagery |
| Lunar Terrain | LOLA elevation |
| Landing-Site Analysis | LOLA and illumination data |
| Orbital Visualisation | JPL Horizons |
| Earth Visualisation | NASA GIBS |
| Mission Safety | CRaTER measurements |
| Scientific Research | PDS archives |

The architecture is intended to keep external data retrieval separate from gameplay logic.

This makes individual integrations easier to maintain, replace, and expand.

# 9. Technology Stack

GIBBUS is being developed using the following technologies:

| Technology | Application |
|---|---|
| Unity | Primary game engine |
| C# | Gameplay programming and simulation systems |
| Unity UI | Interactive mission-design interfaces |
| NASA Open Data | Scientific and mission-related information |
| REST APIs | Integration of externally hosted datasets |
| WMTS | Retrieval of tiled planetary imagery |
| JSON | Structured scientific data |
| Git | Version control |
| GitHub | Collaborative development and source hosting |

**Target platform:** Windows PC.

The development prioritises modular systems, reusable components, and a structured architecture that can support future expansion.

# 10. User Interface

The mission-design environment is organised into structured stages.

The current interface architecture includes:

- Stage headers displaying the current stage number, title, and description.
- A navigation panel for switching between configuration sections.
- Dedicated content panels for individual engineering systems.
- Previous and Next navigation controls.
- A progress indicator.

The Stage 02 configuration interface is organised around five principal sections:

1. Launch System.
2. Spacecraft Architecture.
3. Scientific Payload.
4. Engineering Budget.
5. Mission Approval.

The interface is intended to make complex engineering information understandable without removing the underlying technical considerations.

# 11. Educational Value

GIBBUS demonstrates that space exploration is fundamentally an interdisciplinary engineering challenge.

Through its gameplay, players encounter concepts from:

- Physics and mechanics.
- Mathematics and applied calculations.
- Aerospace engineering.
- Computer science.
- Astronomy and planetary science.
- Systems engineering.
- Scientific research.
- Resource and project management.

Instead of presenting these subjects as disconnected topics, the game connects them through a single mission.

Players learn by making decisions, examining their consequences, and improving their designs.

# 12. Development Roadmap

The project follows a modular development approach.

| Development Phase | Main Focus |
|---|---|
| Phase 1 | Mission-design interface and navigation |
| Phase 2 | Launch system and spacecraft configuration |
| Phase 3 | Scientific payload and engineering calculations |
| Phase 4 | Mission validation and resource management |
| Phase 5 | NASA data integration |
| Phase 6 | Mission simulation and execution |
| Phase 7 | Testing, optimisation, and presentation |

The initial development priority is a functional mission-design experience, establishing the foundation for more advanced simulation features.

# 13. Vision

GIBBUS is built around a simple idea:

**Understanding space exploration should involve more than observing it. People should be able to experiment with the engineering decisions that make it possible.**

The long-term vision is to develop GIBBUS into an expandable educational space-mission simulation environment.

Future campaigns could explore different mission architectures, scientific objectives, destinations, and engineering challenges.

The project is intended to grow beyond a single lunar expedition into a platform for learning about the systems and decisions involved in humanity's exploration of space.

# 14. Project Information

| Detail | Information |
|---|---|
| Project | GIBBUS |
| Initial Campaign | GIBBUS-01 |
| Competition | NASA International Space Apps Challenge 2026 |
| Challenge | Space Mission Design Game |
| Development Team | JETSTACK |
| Genre | Educational / Simulation / Space Exploration |
| Engine | Unity |
| Programming Language | C# |
| Platform | Windows PC |

---

# 15. Acknowledgements and Data Attribution

GIBBUS acknowledges the scientific institutions and missions responsible for making planetary research and space-science information publicly accessible.

Key data providers and missions include:

- NASA Jet Propulsion Laboratory (JPL).
- NASA Lunar Reconnaissance Orbiter (LRO).
- Lunar Orbiter Laser Altimeter (LOLA).
- NASA Planetary Data System (PDS).
- NASA Moon Trek.
- NASA Global Imagery Browse Services (GIBS).
- NASA Satellite Situation Center.
- Cosmic Ray Telescope for the Effects of Radiation (CRaTER).
- Washington University in St. Louis PDS Geosciences Node.

Individual datasets and imagery should be attributed according to their respective provider's acknowledgement and usage requirements.

---

# 16. Important Development Disclaimer

GIBBUS is a work in progress.

The presence of a service in this documentation indicates that it has been identified as a relevant data source for the project. It does not necessarily mean that the service has already been integrated into the current build.

Similarly, proposed simulation features are not representations of completed functionality unless demonstrated in the current version.

Scientific datasets may require additional processing, validation, coordinate conversion, and interpretation before they can be used in gameplay.

GIBBUS aims to distinguish authentic scientific information from simulated values and game-design assumptions.

---

**Built by JETSTACK.**

*Design. Engineer. Explore.*
