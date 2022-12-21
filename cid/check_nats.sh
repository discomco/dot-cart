#!/bin/bash

response=$(curl -s http://"$1":8222/healthz)

echo "$response"

if [ "$response" == '{"status":"ok"}' ]; then
  # NATS is up
  echo "NATS is up"
  exit 0
else
  # NATS is not up
  echo "NATS is not up"
  exit 1
fi
