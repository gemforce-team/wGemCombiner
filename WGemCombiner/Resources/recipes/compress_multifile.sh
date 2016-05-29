#!/bin/bash

destname=${1}
filelist=( "${@:2}" )

if [ "${#filelist[@]}" -ne 0 ]
then
    cat "${filelist[@]}"       |  # Cat all files
        grep -E "^$|^\(val ="  |  # Keep only equation lines or empty lines
        sed '/./,$!d'          |  # Remove leading blank lines at top of file
        cat -s                 |  # Remove double linefeeds
        tr -d ' \t'            |  # Remove whitespace
        sed 's/(.*=//g'        |  # Remove up to '=' then Remove 'g1'
        sed 's/g1//g'          >  "${destname}"
else
    echo "The file list is empty"
fi
