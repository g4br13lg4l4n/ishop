---
name: angular-21-http-practices
description: Applies Angular 21 HTTP Client and httpResource best practices when writing or reviewing Angular services, API calls, or reactive data fetching. Use when working with HttpClient, httpResource, fetch, signals, or Angular 21 HTTP docs.
---

# Angular 21 HTTP & httpResource Practices

Use this skill when implementing or reviewing HTTP calls, services, or reactive data fetching in Angular 21+. Official refs: [Making requests](https://angular.dev/guide/http/making-requests), [httpResource](https://angular.dev/guide/http/http-resource).

## When to use what

| Use case | Prefer | Notes |
|----------|--------|--------|
| **Read data** (GET) that depends on signals/inputs | `httpResource` | Reactive: re-fetches when dependencies change; exposes `value`, `error`, `isLoading`, `hasValue()` as signals. |
| **Mutations** (POST, PUT, PATCH, DELETE) | `HttpClient` | httpResource is not recommended for mutations. Use `http.post()`, etc., and subscribe or `toSignal`. |
| **One-off or imperative GET** | `HttpClient.get()` | Subscribe or use `async` pipe / `toSignal`. |
| **Full response** (headers, status) | `HttpClient.get(..., { observe: 'response' })` | Type: `HttpResponse<T>`. |
| **Progress / events** | `HttpClient` with `observe: 'events'`, `reportProgress: true` | Use for upload/download progress. |

## HttpClient quick rules

- **Generic type**: `http.get<Config>(url)` — type assertion only; server is not validated.
- **Params**: Prefer object literal `{ params: { filter: 'all' } }`; for dynamic params use `HttpParams` (immutable: `.set()` returns new instance).
- **Headers**: Object literal or `HttpHeaders` (immutable).
- **Subscribe**: Requests run only on subscribe. Multiple subscriptions = multiple requests. Prefer `async` pipe or `toSignal()` for cleanup.
- **Errors**: All failures (network, timeout, 4xx/5xx) go to the Observable error channel as `HttpErrorResponse`. Use `catchError`, `retry()` as needed.
- **Timeout**: `http.get(url, { timeout: 3000 })` — aborts after N ms (request only, not interceptors).
- **Response type**: `responseType: 'json' | 'text' | 'blob' | 'arraybuffer'`; use literal type (e.g. `'text' as const`) if options are in a variable.

## httpResource quick rules

- **Reactive argument**: Pass a function that returns a URL string or a request object. Dependencies (signals/inputs) go inside that function so the resource re-runs when they change.
- **Eager**: Starts the request immediately (unlike HttpClient which waits for subscribe).
- **Signals**: Use `resource.value()`, `resource.error()`, `resource.isLoading()`, `resource.hasValue()`. Guard `value()` with `hasValue()` to avoid throwing in error state.
- **Template**: Use `@if (resource.hasValue()) { ... } @else if (resource.error()) { ... } @else if (resource.isLoading()) { ... }`.
- **Mutations**: Do not use httpResource for POST/PUT/PATCH/DELETE; use HttpClient.
- **Parse/validate**: Optional `parse` option (e.g. Zod schema) to validate response and type `value()`.

## Service structure

- Prefer **injectable services** for API access; avoid calling HttpClient directly from components when logic can be reused.
- Expose **signals** (e.g. from httpResource or `toSignal(http.get(...))`) so components stay reactive and templates simple.
- For **mutations**, return `Observable` from the service and let the component subscribe or use `toSignal()`; do not mix imperative `.subscribe()` in services with httpResource signals unless intentional.

## Fetch-specific options (Angular 21 with fetch)

When using `provideHttpClient(withFetch())`:

- **Cache**: `cache: 'force-cache' | 'no-cache' | 'only-if-cached'`
- **Priority**: `priority: 'high' | 'low' | 'auto'` (e.g. LCP-critical vs analytics)
- **Credentials**: `credentials: 'include' | 'omit' | 'same-origin'`
- **Keepalive**: `keepalive: true` so request outlives the page (e.g. analytics)

## References

- [Making requests](https://angular.dev/guide/http/making-requests) — params, headers, observe, responseType, errors, timeouts, fetch options.
- [Reactive data fetching with httpResource](https://angular.dev/guide/http/http-resource) — signals, parse/validation, testing.
