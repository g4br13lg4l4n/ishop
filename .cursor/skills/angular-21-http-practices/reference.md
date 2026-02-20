# Angular 21 HTTP — Reference

## HttpClient examples

```ts
// GET with type and params
http.get<Config>('/api/config', { params: { filter: 'all' } }).subscribe((config) => { /* ... */ });

// POST (mutation)
http.post<Config>('/api/config', newConfig).subscribe((config) => { /* ... */ });

// Full response (headers, status)
http.get<Config>('/api/config', { observe: 'response' }).subscribe((res) => {
  console.log(res.status, res.body);
});

// Timeout
http.get('/api/config', { timeout: 3000 }).subscribe({ next: (c) => {}, error: (err) => {} });
```

## httpResource examples

```ts
// Simple GET — reactive to userId
userId = input.required<string>();
user = httpResource(() => `/api/user/${userId()}`);

// Request object with options
user = httpResource(() => ({
  url: `/api/user/${userId()}`,
  method: 'GET',
  params: { fast: 'yes' },
  headers: { 'X-Special': 'true' },
}));

// Template usage
// @if (user.hasValue()) { <user-details [user]="user.value()"> }
// @else if (user.error()) { <div>Could not load user</div> }
// @else if (user.isLoading()) { <div>Loading...</div> }
```

## Links

- [Making requests](https://angular.dev/guide/http/making-requests)
- [httpResource (reactive data fetching)](https://angular.dev/guide/http/http-resource)
