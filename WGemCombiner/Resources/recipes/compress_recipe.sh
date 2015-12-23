#!/bin/bash

dest=$1

file="long/"${dest}
if [ -s "${file}" ]
then
    cat -s "${file}"     |  # Cat and remove double whitelines
        sed '/^#/d'      |  # Remove comment lines
        tr -d ' \t'      |  # Remove whitespace
        sed 's/(.*=//g'  |  # Remove up to '=' then Remove 'g1'
        sed 's/g1//g'    >  "${dest}"
else
    echo "${file} does not exist"
fi
