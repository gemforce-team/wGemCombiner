#!/bin/bash

# Rebuilds recipe files from a valid gemforce results folder
# Gathers the correct table from the files themselves
# MUST be run from the "recipes" directory

if [ "$#" -ne 1 ]; then
    echo "You must give the results folder path"
    exit
fi

results_folder=$1

# Leech
./compress_multifile.sh leech.txt  $(echo "${results_folder}/leech-combine/"lc*.txt | sort)

# Bbound
:

# Mgcomb exact
./compress_multifile.sh mgcomb-exact.txt  $(grep -rl table_mgcexact  "${results_folder}/managem-combine/" | sort)
# Mgcomb
./compress_multifile.sh mgcomb.txt        $(grep -rlE table_mgc6\|table_mgcomb "${results_folder}/managem-combine/" | sort)
# Mgcomb leech
:

# Mgspec exact
./compress_multifile.sh mgspec-exact.txt  $(grep -rl table_mgsexact  "${results_folder}/managem-amps/" | sort)
# Mgspec appr
./compress_multifile.sh mgspec-appr.txt   $(grep -rl table_mgsappr   "${results_folder}/managem-amps/" | sort)

# Kgcomb exact
./compress_multifile.sh kgcomb-exact.txt  $(grep -rl table_kgcexact  "${results_folder}/killgem-combine/" | sort)
# Kgcomb
./compress_multifile.sh kgcomb.txt        $(grep -rlE table_kgc6\|table_kgcomb "${results_folder}/killgem-combine/" | sort)
# Kgcomb bbound
:

# Kgspec exact
./compress_multifile.sh kgspec-exact.txt     $(grep -rl table_kgsexact  "${results_folder}/killgem-amps/" | sort)
# Kgspec appr
./compress_multifile.sh kgspec-appr.txt      $(grep -rl table_kgsappr   "${results_folder}/killgem-amps/" | sort)
# Kgspec mgsexact
./compress_multifile.sh kgspec-mgsexact.txt  $(grep -rl table_mgsexact  "${results_folder}/killgem-amps/" | sort)
# Kgspec kgssemi
./compress_multifile.sh kgspec-kgssemi.txt   $(grep -rl table_kgssemi   "${results_folder}/killgem-amps/" | sort)
# Kgspec mgsappr
./compress_multifile.sh kgspec-mgsappr.txt   $(grep -rl table_mgsappr   "${results_folder}/killgem-amps/" | sort)

# GES Kgspec exact
:
# GES Kgspec appr
:
# GES Kgspec mgsexact
:
# GES Kgspec kgssemi
:
# GES Kgspec mgsappr
:
