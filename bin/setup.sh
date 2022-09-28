#!/bin/bash
set -euo pipefail

# Initialize k3d

if ! k3d cluster list | grep dapr-cluster > /dev/null; then
    k3d cluster create -c k3d.yaml
fi

# Initialize in K8s
dapr init -k

# Build Images
sh $(pwd)/bin/redeploy.sh

