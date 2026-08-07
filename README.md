# EF Core Interceptors Demo

A small, deliberately "stupid" demo application that exists for exactly one reason: to show off a little bit of what you can do with **EF Core interceptors**. It is not a good example of application architecture, it is not production code, and it should not be used as a template for a real project. Its entities, pages, and "shop" concept are just a thin excuse to have something to intercept.

This repo was built as companion material for a EF Core Community Stand Up about interceptors. If you're watching (or have watched) that video, this is the code referenced in it.

## What is this thing?

It's a very simple ASP.NET Core Razor Pages "shop" with product categories and products, backed by SQL Server through EF Core. It supports a handful of hard-coded users and a fake multi-tenancy setup, purely so there's something interesting for the interceptors to react to:

- Logging in as different users.
- Being routed to different databases depending on which "tenant" you're logged in as.
- Editing products/categories as an admin and having audit fields stamped automatically.
- Loading related data (products for a category) through a couple of different, EF Core interceptor-powered patterns.

None of that is the point of the app. The point is the interceptors.

## The interceptors

All of them live in [`src/FiftyNine.EfCore.InterceptorDemo.Web/Data/Interceptors`](src/FiftyNine.EfCore.InterceptorDemo.Web/Data/Interceptors) and are wired up where the `DbContext` is registered in [`Program.cs`](src/FiftyNine.EfCore.InterceptorDemo.Web/Program.cs), via `.AddInterceptors(...)`.

| Interceptor | Interface | What it demos |
|---|---|---|
| `UserContextConnectionInterceptor` | `IDbConnectionInterceptor` | Swaps out the SQL connection at connection-creation time, based on the current user's tenant. This is how the app fakes multi-tenancy: one `DbContext`, many possible databases. |
| `ChangeTrackingInterceptor` | `ISaveChangesInterceptor` | Stamps `LastModifiedBy`/`LastModifiedAt` shadow properties onto added/modified entities just before `SaveChanges` runs — a classic auditing use case. |
| `ProductsProviderInjectionInterceptor` | `IMaterializationInterceptor` | Hijacks `CreatingInstance` to construct a `ProductCategory` via its non-public constructor, injecting a helper bound to the current `DbContext`. |
| `ProductsProviderPropertyInterceptor` | `IMaterializationInterceptor` | An alternative to the above: instead of controlling construction, it sets a property on `InitializedInstance` after the entity already exists. |
| `ILoadProductsInterceptor` | `IMaterializationInterceptor` | Yet another alternative: wires up a lazy-load delegate on entities implementing `ILoadProducts`, so related products can be fetched on demand. |

The three materialization interceptors are **alternative solutions to the same problem** (getting query-related helpers/data onto an entity as it's materialized) — only one is meant to be active at a time. Open `Program.cs` and you'll see the other two commented out; swap them to see the different approaches in action.

## Getting started

### Prerequisites

- .NET 10 SDK
- Docker Desktop (or another OCI-compatible container runtime) — .NET Aspire uses it to spin up SQL Server for you

### Running it

The solution uses **.NET Aspire** to orchestrate everything, so you don't need a local SQL Server install or a `docker-compose` file — Aspire provisions a SQL Server container with three databases (`Default`, `Kite`, `Wingfoil`, simulating separate tenants).

1. Clone the repo.
2. Make sure Docker Desktop is running.
3. Run the AppHost project:

   ```bash
   dotnet run --project src/Aspire/FiftyNine.EfCore.InterceptorDemo.AppHost
   ```

4. Open the Aspire dashboard URL printed in the console to see the resources come up, or just navigate straight to the `web` project's URL shown there.

On startup, a couple of hosted services take care of the rest:

- `MigrationRunnerService` applies EF Core migrations to every configured database.
- `SeedDataService` seeds each tenant's database with some demo categories and products if it's empty.

Log in with one of the demo users to see different tenants/roles in action (check `IUsers`/`InMemoryUsers` in the `Data` folder for the available accounts). Admin pages live under `/Admin` and require the `Admin` claim.

## Project structure

- **`src/Aspire/FiftyNine.EfCore.InterceptorDemo.AppHost`** — the Aspire orchestrator. Defines the SQL Server container, its three databases, and the web app that references them.
- **`src/Aspire/FiftyNine.EfCore.InterceptorDemo.ServiceDefaults`** — standard Aspire service defaults (telemetry, health checks, resilience, service discovery).
- **`src/FiftyNine.EfCore.InterceptorDemo.Web`** — the actual demo app: Razor Pages, `DemoDbContext`, entities, and — the whole point of this repo — the interceptors.

## A word of warning

This code cuts corners everywhere it possibly can. Hard-coded users, no real security hardening, questionable domain modeling, reflection used just to prove a point — all on purpose, all so the interceptor code stays front and center. Please don't copy the surrounding scaffolding into a real project. Copy the interceptor ideas, not the shop.

## Questions? Feedback?

Found a bug in the demo, have a question about how one of the interceptors works, or just want to say hi? Reach out — I'm happy to chat about EF Core, interceptors, or more or less anything else tech related you would want to talk about.

- Twitter/X: [@ZeroKoll](https://twitter.com/ZeroKoll)
- Bluesky: [@zerokoll.bsky.social](https://bsky.app/profile/zerokoll.bsky.social)
- Email: chris@59north.com
