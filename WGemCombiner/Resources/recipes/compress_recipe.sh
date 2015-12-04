#!/bin/bash

dest=$1

cat -s long/${dest}    |  # Cat and remove double whitelines
    sed '/^#/d'        |  # Remove comment lines
    tr -d ' \t'        |  # Remove whitespace
    sed 's/(.*=//g'    |  # Remove up to '=' then Remove 'g1'
    sed 's/g1//g'      >  ${dest}
