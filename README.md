# Pulse

A distributed synthetic monitoring system that checks reachability and correctness of services from multiple locations.



## Core Architecture

        ┌──────────────────────┐
        │   Pulse.Dashboard    │ (Blazor UI)
        └──────────┬───────────┘
                   │
                   ▼
        ┌──────────────────────┐
        │     Pulse.Server     │ (ASP.NET API)
        │  - Control Plane     │
        │  - Data Storage      │
        └──────────┬───────────┘
                   ▲
                   │ HTTP (pull config / push results)
                   │
        ┌──────────┴───────────┐
        │     Pulse.Agent      │ (Worker Service)
        │  - Executes checks   │
        └──────────────────────┘


**Explanation:**

*   **Pulse.Dashboard:**  The user interface built with Blazor.
*   **Pulse.Server:** The backend API built with ASP.NET.  Responsible for control and data storage.
*   **Pulse.Agent:**  A worker service that performs the actual checks.
*   **Communication:**  The components communicate primarily via HTTP requests (pulling configuration from the server and pushing results).
