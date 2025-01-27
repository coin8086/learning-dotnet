# Using PostgreSQL

1. Select and pull a docker image from https://hub.docker.com/_/postgres/, like

   ```
   docker image pull postgres:17.2
   ```

1. Run PostgreSQL in container like

   ```
   docker run --name pgtest -e POSTGRES_PASSWORD=*** -p 127.0.0.1:5432:5432 -d postgres:17.2
   ```

1. Set environment variables `PG_CONNECTION` and `PG_VERSION`, like
   
   ```ps1
   $env:PG_CONNECTION = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=***"
   $env:PG_VERSION = "17.2"
   ```

To stop the container, use `docker container stop`. To remove a stopped container, use `docker container rm` or `docker container prune`.
