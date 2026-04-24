FROM debian:bookworm-20260421 AS base

RUN apt-get update && \
    apt-get install -y cron postgresql-client

FROM base AS final
WORKDIR /src
COPY . .

RUN chmod +x ./jobs/clear_orphan_rooms.sh
RUN echo "* * * * * /src/jobs/clear_orphan_rooms.sh >> /src/output.log" | crontab -
CMD ["cron", "-f"]