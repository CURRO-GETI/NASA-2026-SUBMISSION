# 🚀 GIBBOUS
### Design. Engineer. Explore.

**An interactive space mission design and simulation experience.**

GIBBOUS is an educational space exploration and mission-design simulator developed by **JETSTACK** for the 2026 NASA International Space Apps Challenge, under the *Space Mission Design Game* challenge.

The project transforms the complex process of designing a space mission into an interactive experience. Players take responsibility for engineering decisions, scientific objectives, spacecraft configuration, resource management, and mission planning.

Rather than simply teaching players about space exploration, GIBBOUS places them in the role of mission designers, allowing them to experiment with the decisions, constraints, and compromises involved in planning a space expedition.

The project combines interactive simulation, aerospace engineering concepts, and NASA planetary-science resources to create an educational experience grounded in real scientific information.

---

# 1. Project Overview

Designing a successful space mission requires the coordination of numerous interconnected engineering systems.

A spacecraft must satisfy mass, power, propulsion, communications, and operational requirements while remaining within the limitations of its launch vehicle and available resources.

Every engineering decision introduces consequences.

Increasing scientific payload mass affects spacecraft requirements. Selecting a launch vehicle influences mission capacity. Propulsion requirements influence fuel allocation, while orbital conditions and environmental constraints affect mission planning.

GIBBOUS brings these interconnected challenges into one interactive environment.

Players progress through a structured mission-design process, configuring the systems required to develop a viable space expedition.

The initial campaign, **GIBBOUS-01**, focuses on a lunar science expedition.

Its mission objective is to plan a journey from Earth to the Moon, conduct scientific operations, and safely return to Earth.

# 2. Project Objectives

GIBBOUS aims to:

- Make aerospace engineering more accessible through interactive gameplay.
- Introduce players to real-world spacecraft and mission-design principles.
- Demonstrate the relationship between engineering decisions and mission success.
- Encourage scientific curiosity and critical thinking.
- Incorporate authentic NASA datasets and scientific information.
- Provide an environment for experimenting with different mission architectures.
- Demonstrate how mathematics, physics, computer science, and engineering work together in space exploration.

# 3. Gameplay and Mission Design

GIBBOUS follows the development of a space mission through a series of structured configuration and evaluation stages.

Players take on the role of mission designers, making decisions that influence the feasibility, performance, and scientific objectives of their expedition.

## 3.1 Mission Design Workflow

The mission-design framework consists of nine stages:

| Stage | Description |
|---|---|
| 01. Mission Objectives | Define the purpose, scientific goals, and requirements of the expedition. |
| 02. Launch Vehicle Selection | Select an appropriate launch system based on mission requirements and payload capacity. |
| 03. Spacecraft Architecture | Configure the spacecraft and determine how its components work together. |
| 04. Scientific Payload | Select scientific instruments and experiments according to mission objectives. |
| 05. Resource Management | Balance mass, power, propulsion, and financial constraints. |
| 06. Communications | Establish communication requirements between the spacecraft and mission control. |
| 07. Trajectory Design | Consider orbital requirements and the spacecraft's journey. |
| 08. Contingency Planning | Consider potential complications, operational risks, and mission contingencies. |
| 09. Mission Validation | Evaluate the completed design against the mission requirements. |

Each stage contributes to the overall mission architecture.

The objective is to encourage players to understand that spacecraft engineering is a systems-level discipline in which individual decisions affect the entire mission.

## 3.2 Mission Execution

Following mission validation, the planned mission-execution experience covers:

1. Launch preparation and liftoff.
2. Atmospheric ascent.
3. Orbital insertion.
4. Translunar journey.
5. Lunar approach and landing.
6. Scientific operations.
7. Lunar ascent and return journey.
8. Earth re-entry.
9. Mission completion and evaluation.

The intended simulation connects mission-design decisions with the operational challenges encountered during the expedition.

# 4. Engineering Systems

GIBBOUS focuses on interconnected engineering systems rather than treating spacecraft components as isolated selections.

| Engineering System | Purpose |
|---|---|
| Launch Systems | Determine launch capabilities and payload capacity. |
| Spacecraft Architecture | Define spacecraft configuration and modules. |
| Scientific Payload | Provide instruments required for mission objectives. |
| Mass Management | Evaluate spacecraft mass against design constraints. |
| Power Management | Account for electrical power requirements. |
| Propulsion | Consider propulsion capabilities and fuel requirements. |
| Communications | Establish spacecraft communication requirements. |
| Orbital Mechanics | Introduce orbital and trajectory considerations. |
| Mission Budget | Manage financial limitations. |
| Mission Safety | Consider environmental and operational risks. |

## 4.1 Engineering Trade-offs

The engineering systems are designed around the principle that individual decisions have interconnected consequences.

For example:

- Increasing payload mass affects launch requirements.
- Additional scientific instruments introduce power and mass requirements.
- Propulsion requirements influence fuel allocation.
- Landing-site selection introduces terrain, illumination, and environmental considerations.
- Communication requirements influence spacecraft operations.

These trade-offs form the foundation of GIBBOUS's mission-design experience.

# 5. GIBBOUS-01: Lunar Expedition

GIBBOUS-01 is the initial campaign and primary demonstration mission.

It focuses on a lunar expedition incorporating spacecraft configuration, lunar exploration, scientific research, and Earth return operations.

The mission architecture is based on three principal spacecraft elements:

### Crew Return Spacecraft

Provides the crew with a habitable vehicle for the journey and return to Earth.

### Service Module

Supports essential spacecraft operations, including propulsion and other supporting systems.

### Lunar Lander

Enables the crew to travel between lunar orbit and the lunar surface.

The player must consider how these elements interact and whether the selected architecture can support the intended expedition.

GIBBOUS-01 provides a practical context for exploring the engineering requirements of a lunar mission.

# 6. NASA Data Integration

## 6.1 Overview

One of the defining technical aspects of GIBBOUS is its use of real planetary-science information and NASA resources.

The project incorporates lunar imagery, geographical information, and surface-elevation information to provide a scientific foundation for its interactive lunar environment.

Rather than relying entirely on fictional terrain, GIBBOUS uses NASA-derived information to support lunar exploration and landing-site evaluation.

The principal scientific data categories include:

- Lunar surface imagery and mapping.
- Lunar surface elevation and topography.
- Lunar geographical features and landmarks.
- Lunar landing-site characteristics.
- Planetary and lunar orbital information.
- Radiation and environmental considerations.
- Scientific mission archives.

## 6.2 Lunar Surface Imagery and Mapping

### NASA Moon Trek – Lunar Reconnaissance Orbiter Wide Angle Camera (LRO WAC)

**Provider:** NASA / Jet Propulsion Laboratory  
**Service:** NASA Moon Trek  
**Protocol:** Web Map Tile Service (WMTS)

NASA Moon Trek provides access to lunar imagery and scientific mapping resources.

GIBBOUS uses lunar imagery to support its interactive three-dimensional Moon environment.

**Official resource:**

https://trek.nasa.gov/moon/

**WMTS capabilities endpoint:**

https://trek.nasa.gov/tiles/Moon/EQ/LRO_WAC_Mosaic_Global_303ppd_v02/1.0.0/WMTSCapabilities.xml

**API documentation:**

https://trek.nasa.gov/tiles/apidoc/trekAPI.html?body=moon

**Example imagery tile:**

https://trek.nasa.gov/tiles/Moon/EQ/LRO_WAC_Mosaic_Global_303ppd_v02/1.0.0/default/default028mm/0/0/0.jpg

**Data characteristics:**
- Global lunar surface imagery.
- LRO Wide Angle Camera mosaics.
- Geographical surface information.
- Tiled imagery at different zoom levels.
- Multiple lunar mapping layers.

**Application in GIBBOUS:**

The imagery provides the visual foundation for the interactive lunar globe.

Players can explore the lunar surface and investigate geographical regions when considering potential landing destinations.

The use of tiled imagery also provides a pathway for more detailed regional visualisation.

## 6.3 Lunar Elevation and Topography

### NASA Lunar Orbiter Laser Altimeter (LOLA)

**Mission:** Lunar Reconnaissance Orbiter  
**Instrument:** Lunar Orbiter Laser Altimeter  
**Archive:** NASA Planetary Data System Geosciences Node

LOLA is a laser altimeter instrument aboard NASA's Lunar Reconnaissance Orbiter.

Its measurements provide scientific information about lunar surface elevation and topography.

**Dataset archive:**

https://pds-geosciences.wustl.edu/missions/lro/lola.htm

**Orbital Data Explorer:**

https://ode.rsl.wustl.edu/moon/

**ODE REST endpoint:**

https://oderest.rsl.wustl.edu/live2/

**Data characteristics:**
- Lunar surface elevation.
- Digital elevation models.
- Topographical information.
- Surface height measurements.
- Geographical data products.

**Application in GIBBOUS:**

Lunar elevation information supports the project's exploration of realistic lunar surface characteristics.

Processed elevation data can be represented as a heightmap and applied to a spherical terrain mesh within Unity.

This provides a foundation for evaluating landing-site characteristics such as:

- Surface elevation.
- Terrain variation.
- Slope.
- Potential landing hazards.

Scientific elevation products require appropriate processing and coordinate conversion before they can be used as Unity terrain inputs.

## 6.4 Lunar Landing-Site Evaluation

Landing-site selection is an important part of GIBBOUS's lunar exploration experience.

Rather than choosing a landing destination based solely on appearance, players are encouraged to consider scientific and environmental characteristics.

The evaluation framework considers factors including:

- Surface geography.
- Elevation.
- Terrain suitability.
- Lunar illumination.
- Environmental conditions.
- Mission requirements.
- Orbital considerations.
- Propulsion and fuel requirements.

The purpose is to demonstrate that selecting a lunar landing location involves several interconnected scientific and engineering considerations.

## 6.5 Lunar Illumination

### NASA Moon Trek – South Pole Illumination

**Provider:** NASA / JPL  
**Service:** Moon Trek WMTS

Lunar illumination is an important consideration when planning operations near the lunar south pole.

Sunlight availability can influence surface operations, thermal conditions, and the feasibility of solar-powered equipment.

**WMTS capabilities endpoint:**

https://trek.nasa.gov/tiles/Moon/SP/WAC_POLE_ILL_PCT_SOUTH_100M/1.0.0/WMTSCapabilities.xml

**Dataset layer:**

WAC_POLE_ILL_PCT_SOUTH_100M

**Application in GIBBOUS:**

Illumination information can support landing-site evaluation by providing context about sunlight availability.

This introduces another engineering consideration when choosing an appropriate destination.

## 6.6 Planetary Positions and Orbital Information

### NASA JPL Horizons System

**Provider:** NASA Jet Propulsion Laboratory  
**Service:** JPL Horizons API  
**Protocol:** REST API

JPL Horizons provides astronomical ephemerides and information about the positions and velocities of celestial bodies.

**Direct API endpoint:**

https://ssd.jpl.nasa.gov/api/horizons.api

**Documentation:**

https://ssd-api.jpl.nasa.gov/doc/horizons.html

**Data characteristics:**
- Planetary positions.
- Lunar positions.
- Velocity vectors.
- Time-dependent astronomical information.
- Ephemeris calculations.

**Application in GIBBOUS:**

Astronomical position information provides a scientific reference for planetary and lunar orbital visualisation.

It can support the representation of the Earth-Moon system and provide astronomical inputs for mission-planning calculations.

Importantly, ephemeris information does not independently calculate a spacecraft's complete trajectory or fuel requirements. Those require additional propulsion and orbital-dynamics calculations.

## 6.7 Radiation and Space Environment

### NASA Lunar Reconnaissance Orbiter – CRaTER

**Mission:** Lunar Reconnaissance Orbiter  
**Instrument:** Cosmic Ray Telescope for the Effects of Radiation

CRaTER measures radiation in the lunar environment and contributes to scientific understanding of radiation exposure during space exploration.

**Scientific data portal:**

https://crater-web.sr.unh.edu/

**Application in GIBBOUS:**

Radiation is an important consideration for crewed space missions.

The scientific information provides context for evaluating radiation exposure and the importance of spacecraft protection.

It also creates opportunities to introduce radiation shielding and crew safety as engineering considerations.

CRaTER scientific products are archived datasets and require interpretation and processing before they can be used in a simulation.

## 6.8 Scientific Data Discovery

### NASA Planetary Data System (PDS)

**Provider:** NASA  
**Service:** PDS Search API  
**Protocol:** REST API

The Planetary Data System provides access to scientific planetary datasets and mission archives.

**API endpoint:**

https://pds.nasa.gov/api/search/1/

**Documentation:**

https://nasa-pds.github.io/pds-api/guides/search/endpoints.html

**Application in GIBBOUS:**

PDS provides a scientific data-discovery resource for identifying relevant planetary datasets, retrieving metadata, and locating archived scientific products.

It is particularly useful when expanding the range of scientific information available to the project.

PDS is an archive discovery and access system, not a direct source of continuously updated lunar imagery.

# 7. Scientific Data and Simulation Architecture

GIBBOUS uses a modular approach to separating scientific data retrieval, processing, and gameplay systems.

The intended architecture follows this structure:

```text
             NASA DATA SOURCES
                     |
          +----------+----------+
          |          |          |
       Moon Trek   JPL        PDS / ODE
          |       Horizons       |
          |          |          |
          +----------+----------+
                     |
                     |
             DATA RETRIEVAL
                     |
                     |
             DATA PROCESSING
                     |
          +----------+----------+
          |          |          |
       Imagery   Elevation   Ephemerides
          |          |          |
          +----------+----------+
                     |
                     |
              UNITY SYSTEMS
                     |
          +----------+----------+
          |          |          |
       Moon Globe  Mission   Landing-Site
                  Planning    Evaluation
          |          |          |
          +----------+----------+
                     |
                     |
              PLAYER EXPERIENCE
```

## 7.1 Data Retrieval

Dedicated integration components are responsible for communicating with external services.

Potential component responsibilities include:

- Moon imagery retrieval.
- Lunar elevation retrieval.
- Scientific dataset discovery.
- Astronomical ephemeris retrieval.
- Environmental data retrieval.

## 7.2 Data Processing

Scientific information is not always immediately compatible with Unity.

Depending on the source, processing can involve:

- JSON deserialization.
- Image tile retrieval.
- Heightmap conversion.
- Coordinate transformations.
- Unit conversion.
- Data interpolation.
- Local caching.

## 7.3 Unity Integration

Processed scientific information can then be supplied to the relevant gameplay systems.

| Unity System | Scientific Information |
|---|---|
| Lunar Globe | NASA lunar imagery |
| Lunar Terrain | Elevation and topography |
| Landing-Site Evaluation | Geography and terrain information |
| Orbital Visualisation | Astronomical ephemerides |
| Mission Planning | Scientific and engineering constraints |
| Mission Safety | Radiation and environmental information |

An important design principle is the distinction between observed scientific information, derived calculations, and simulated gameplay values.

This helps maintain scientific transparency while allowing the game to remain accessible.

# 8. Technology Stack

GIBBOUS is developed using the following technologies:

| Technology | Application |
|---|---|
| Unity 6 | Primary game engine |
| C# | Gameplay programming and simulation systems |
| Unity UI | Interactive mission-design interfaces |
| REST APIs | External scientific data integration |
| WMTS | Retrieval of tiled planetary imagery |
| JSON | Structured scientific data |
| Git | Version control |
| GitHub | Collaborative development and source hosting |

**Target platform:** Windows PC.

The project prioritises modular development, reusable components, and a structured architecture that can support future expansion.

# 9. User Interface and Navigation

The mission-design environment is organised into structured configuration stages.

The interface architecture includes:

- Stage headers displaying the current stage number, title, and description.
- Navigation panels for accessing different configuration sections.
- Dedicated content panels for individual engineering systems.
- Previous and Next navigation controls.
- Mission progression indicators.
- Interactive configuration elements.

The Stage 02 configuration interface is organised around five principal sections:

1. Launch System.
2. Spacecraft Architecture.
3. Scientific Payload.
4. Engineering Budget.
5. Mission Approval.

The interface aims to make complex engineering information understandable without removing the underlying technical considerations.

# 10. Educational Value

GIBBOUS demonstrates that space exploration is fundamentally an interdisciplinary engineering challenge.

Through its gameplay, players encounter concepts from:

- Physics and mechanics.
- Mathematics and applied calculations.
- Aerospace engineering.
- Computer science.
- Astronomy and planetary science.
- Systems engineering.
- Scientific research.
- Resource and project management.

Instead of presenting these subjects as disconnected topics, GIBBOUS connects them through a single mission.

Players learn by making engineering decisions, examining their consequences, and improving their designs.

# 11. Development Approach

GIBBOUS follows a modular development approach, allowing individual mission systems to be developed and tested independently.

The development priorities include:

| Development Area | Focus |
|---|---|
| Mission Interface | Configuration stages and navigation |
| Launch Systems | Launch vehicle selection |
| Spacecraft Architecture | Spacecraft configuration |
| Scientific Payload | Instrument selection |
| Engineering Systems | Resource and constraint management |
| NASA Data Integration | Scientific data retrieval and visualisation |
| Mission Validation | Evaluation of mission requirements |
| Mission Simulation | Visualisation of mission execution |
| Testing and Optimisation | Stability and performance |

The project is designed to support additional mission campaigns and scientific integrations as development progresses.

# 12. Future Development

The long-term vision for GIBBOUS is to develop an expandable educational space-mission simulation environment.

Potential future additions include:

- Additional planetary exploration campaigns.
- More detailed orbital mechanics.
- Advanced spacecraft propulsion modelling.
- Expanded lunar terrain visualisation.
- More scientific payload options.
- Improved mission risk analysis.
- More detailed spacecraft subsystems.
- Additional planetary datasets.
- Expanded mission execution and evaluation systems.

These additions would extend the project beyond its initial lunar expedition.

# 13. Project Information

| Detail | Information |
|---|---|
| Project Name | GIBBOUS |
| Initial Campaign | GIBBOUS-01 |
| Organisation | JETSTACK |
| Competition | 2026 NASA International Space Apps Challenge |
| Challenge | Space Mission Design Game |
| Genre | Educational / Simulation / Space Exploration |
| Game Engine | Unity 6 |
| Programming Language | C# |
| Target Platform | Windows PC |

# 14. Acknowledgements and Data Attribution

GIBBOUS acknowledges the scientific institutions, researchers, and space missions responsible for making planetary research and scientific information publicly accessible.

Key resources include:

- NASA Jet Propulsion Laboratory.
- NASA Lunar Reconnaissance Orbiter.
- Lunar Orbiter Laser Altimeter.
- NASA Planetary Data System.
- NASA Moon Trek.
- JPL Horizons.
- NASA Cosmic Ray Telescope for the Effects of Radiation.

Individual scientific datasets and imagery should be attributed according to their respective providers' acknowledgement and usage requirements.

# 15. Super Intelligence and Development Assistance

Super Intelligence tools were used as development assistance during the creation of GIBBOUS.

Their use included programming assistance, debugging support, technical research, documentation, and refinement of written project materials.

AI-assisted suggestions and code were reviewed and adapted by the development team.

The project concept, engineering decisions, implementation direction, and final development work were managed by the team.

Any AI-generated imagery, video, audio, or other assets included in the final project should be identified in accordance with the NASA Space Apps Challenge disclosure requirements.

# 16. Development Disclaimer

GIBBOUS is an educational simulation and is not an operational spacecraft-design or mission-certification system.

Scientific datasets, astronomical observations, derived calculations, and simulated gameplay values serve different purposes and should not be interpreted as interchangeable.

External datasets may require processing, validation, coordinate conversion, and interpretation before integration into the game.

Not every service documented in this README necessarily represents an active integration in the current build. Some resources are included as scientific references or potential future integrations.

Mission outcomes and engineering calculations are intended for educational simulation rather than operational spaceflight planning.

---

**Built by JETSTACK.**

*Design. Engineer. Explore.*
