#!/bin/sh

# finds the libpython for an venv if given as the first argument
# Meant to be used in the dockerfile of pythonserivce
# Specifcally finds 3.13
# Needs the enviormental Variables VENV and PYTHONVERISON

. ./.venv/bin/activate
export PYTHONVERISON=3.13
export VENV=.venv
pythonpath=$(dirname $(readlink -f $VENV/bin/python$PYTHONVERSION))/../lib/libpython$PYTHONVERSION.so
export PYTHONNET_PYDLL=$(realpath $pythonpath)
dotnet $1