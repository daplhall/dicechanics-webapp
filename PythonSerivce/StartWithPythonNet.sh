#!/bin/sh

# finds the libpython for an venv if given as the first argument
# Specifcally finds 3.13

. ./.venv/bin/activate
pythonpath=$(dirname $(readlink -f $1/bin/python3.13))/../lib/libpython3.13.so
export PYTHONNET_PYDLL=$(realpath $pythonpath)
dotnet $2