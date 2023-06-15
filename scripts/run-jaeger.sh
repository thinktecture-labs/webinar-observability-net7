#!/bin/sh

# Exit on error
set -e

docker run -e COLLECTOR_OTLP_ENABLED=true \
    -p 16686:16686 -p 4317:4317 -p 4318:4318 -d \
    jaegertracing/all-in-one
