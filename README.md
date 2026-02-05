# RailWatch
An application to track those pesky trains. 

# RailWatch (working title)

A UK rail reliability & live departures tracker for a small invited group, with a path to go public.
Users can save stations and routes, view live departure boards, and build a history of snapshots to analyze delays and disruptions over time.

## North Star

### Goal
Make it easy for a user to:
1) save the stations and routes they care about,
2) quickly check live departures,
3) understand reliability from historical snapshots (delays/cancellations/platform changes).

### MVP (Thin Slice v1)
- A user can manage **Saved Stations** (add/list/remove).
- A user can click a saved station to view a **Station Board** (live departures).
- A user can manage **Saved Routes** (origin/destination + time window).
- The system can **capture and store snapshots** of a station board.
- Basic dashboard shows:
  - saved stations + routes
  - “last updated” time
  - simple reliability stats (e.g., average delay over last 7 days for a station)

> Note: Until National Rail Darwin access is approved, live boards use a mock provider with realistic sample data.
> Darwin integration will be swapped in behind an adapter interface.

## Core Concepts (Domain)

### Saved Station
A station the user wants quick access to.
- Identified by CRS code (e.g., WAT, EUS)
- Optional friendly name
- Used to power the Station Board and snapshot history

### Saved Route
A route the user wants to track (typically between two stations).
- Origin CRS + Destination CRS
- Optional days-of-week + time window (e.g., Mon–Fri, 17:00–19:00)
- Used later for route-specific reliability and alerts

### Station Board
A “live departures” view for a station.
- Departures list (service, destination, platform, scheduled time, estimated time, status)
- Source: Darwin later, mock data initially

### Snapshot
A stored record of what the Station Board looked like at a point in time.
- Captured manually (button) and/or by a background worker (later)
- Enables reliability trends, disruption detection, and alerting

## Data Model (initial)
- User
- SavedStation (OwnerId, CrsCode, Name, CreatedAt)
- SavedRoute (OwnerId, OriginCrs, DestinationCrs, Name, DaysOfWeekMask, TimeWindowStart, TimeWindowEnd)
- ServiceSnapshot (OwnerId?, StationCrs, CapturedAt, PayloadJson, MaxDelayMinutes?, HasDisruption?)
- Incident (OwnerId?, StationCrs/RouteId?, StartedAt, EndedAt?, Severity, Summary) [later]
- AlertRule (OwnerId, Type, ThresholdMinutes, QuietHours, Enabled) [later]

## API Surface (initial)
- Stations
  - GET  /api/stations
  - POST /api/stations
  - DELETE /api/stations/{id}

- Routes
  - GET  /api/routes
  - POST /api/routes
  - PUT  /api/routes/{id}
  - DELETE /api/routes/{id}

- Boards & Snapshots
  - GET  /api/boards/{crs}                         (live board; mock first, Darwin later)
  - POST /api/boards/{crs}/snapshots               (capture snapshot now)
  - GET  /api/snapshots?stationCrs=XXX&from=...&to=...

## UI Pages (initial)
- Dashboard
  - Saved Stations list (click → Station Board)
  - Saved Routes list
  - Latest snapshot / last updated indicators

- Station Board
  - Live departures table
  - Button to capture snapshot
  - Simple trend (e.g., avg delay last 7 days)

- Route Detail (basic / later)
  - Live departures relevant to the route
  - Reliability stats

## Non-Goals (for MVP)
- Complex journey planning / mapping
- Full notifications system
- Multi-operator deep analytics

## Tech Notes (direction)
- Backend: .NET 10 Minimal API + EF Core + SQL database
- Frontend: React (consumes API)
- Integration: Darwin feed via adapter interface (mock initially)
- Auth: none initially; later add cookie-based auth / passkeys for public launch

## Project boundaries

- RailWatch.Domain: pure domain + interfaces. No Infrastructure references.
- RailWatch.Infrastructure: EF Core, external providers, implementations. References Domain.
- RailWatch.Api: HTTP endpoints + DI wiring. References Domain + Infrastructure.
- RailWatch.Worker: background jobs + DI wiring. References Domain + Infrastructure.

## Running the API

## Local development

### Secrets (recommended: dotnet user-secrets)

This project uses **dotnet user-secrets** for local development secrets (do not commit secrets to the repo).

### Set database connection string (API)

## Running the API

### Prereqs
- .NET 10 SDK
- SQL Server (e.g. SQLEXPRESS)

### Configure connection string
Set via user-secrets (recommended):

```powershell
dotnet user-secrets init --project src/RailWatch.Api
dotnet user-secrets set "ConnectionStrings:Default" "<YOUR_CONNECTION_STRING>" --project src/RailWatch.Api
```
Example value above is a placeholder format. Use your own server/credentials.
Do not commit real connection strings.


Run the API:
```bash 
dotnet run --project src/RailWatch.Api
```