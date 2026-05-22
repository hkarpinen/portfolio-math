# portfolio-math

Stateless math service. Handles all numerical computation concerns: unit conversion today, with a natural home for equation solvers, statistical calculators, or any other pure-math feature in the future.

Intentionally no persistence, no messaging, no aggregates — the domain is value objects and deterministic functions.

## What it does

- **Unit conversion** — converts between units across length, weight, temperature, volume, speed, area, and data. Available anonymously — pure deterministic math, no external dependencies.

## Stack

- .NET 8 / ASP.NET Core Web API
- Clean Architecture: Domain → Application → Infrastructure → Client
- No database, no RabbitMQ, no EF Core, no external HTTP calls

## Running locally

```bash
# From repo root
dotnet run --project src/Client
```

Or via the full stack:

```bash
docker compose -f infra/compose.dev.yaml up math
```

## Structure

```
src/
  Domain/          Value objects (ConversionUnit) — no aggregates
  Application/     Query interface (IUnitConversionQuery), DTOs
  Infrastructure/  UnitConversionEngine (in-memory, singleton)
  Client/          ASP.NET Core controllers, FluentValidation validators, DI wiring
```

## API surface

| Controller | Routes | Auth | Purpose |
|---|---|---|---|
| `ConversionController` | `GET /api/math/convert?from=kg&to=lb&value=5` | Anonymous | Unit conversion |
| `ConversionController` | `GET /api/math/convert/units` | Anonymous | List supported units by category |

## CI/CD

Two workflows run on push to `main`:

| Workflow | File | What it does |
|---|---|---|
| **Build & Publish** | `.github/workflows/docker-publish.yml` | Runs `dotnet test`, builds the Docker image, pushes to `ghcr.io/hkarpinen/portfolio-math:latest` |
| **Deploy** | `.github/workflows/deploy.yml` | Triggers after Build & Publish succeeds; SSHes into the server, pulls the new image, and restarts only the `math` container |

### Required GitHub Actions secrets

| Secret | Description |
|---|---|
| `DEPLOY_HOST` | VPS IP address or hostname |
| `DEPLOY_USER` | SSH user on the server |
| `DEPLOY_KEY` | Private SSH key for that user |
| `DEPLOY_PATH` | Absolute path to the infra directory on the server |

## Supported units

| Category | Units |
|---|---|
| Length | `m`, `km`, `cm`, `mm`, `mi`, `yd`, `ft`, `in`, `nm` |
| Weight | `kg`, `g`, `mg`, `t`, `lb`, `oz`, `st` |
| Temperature | `c`, `f`, `k` |
| Volume | `l`, `ml`, `cl`, `dl`, `m3`, `gal`, `qt`, `pt`, `fl_oz` |
| Speed | `m_s`, `km_h`, `mph`, `kt` |
| Area | `m2`, `km2`, `cm2`, `mm2`, `ft2`, `in2`, `ha`, `acre` |
| Data | `b`, `kb`, `mb`, `gb`, `tb` |
