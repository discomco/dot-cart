#! /bin/bash
clear
echo '-----------------------------------'
echo "pushing version [$1] to main branch"
echo '-----------------------------------'
echo 'linting and formatting code...'
dotnet format
git add .
git commit -m "$1" -a
git push  
