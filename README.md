# PROJECT WATERLOO

This is the source code for InfoTracks' Project Waterloo technical test

## Run this locally

Build and run `project-waterloo` with Docker Compose:

```sh
git clone https://github.com/richardtatum/project-waterloo.git project-waterloo
cd project-waterloo
docker compose build
docker compose up
```

Once complete, the site will be available at `localhost:80`.

## Notes

Feel free to adjust the ports as required in the `docker-compose.yml` file.

Note that the environment variable `API_URL` is used as the reverse proxy destination for requests to `/api/*` so needs to be updated if the `ASPNETCORE_URLS` environment variable changes.

## References

-   [Caddy](https://caddyserver.com/docs/) (File server and reverse proxy)
-   [Svelte](https://svelte.dev/) (Frontend framework)

