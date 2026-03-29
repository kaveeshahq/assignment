@echo off
cd /d c:\Users\Asus\Desktop\assignment
set FILTER_BRANCH_SQUELCH_WARNING=1

git filter-branch -f --env-filter ^
  "export GIT_AUTHOR_NAME='Kaveesha Dissanayake'; ^
   export GIT_AUTHOR_EMAIL='90174106+kaveeshahq@users.noreply.github.com'; ^
   export GIT_COMMITTER_NAME='Kaveesha Dissanayake'; ^
   export GIT_COMMITTER_EMAIL='90174106+kaveeshahq@users.noreply.github.com'" ^
  -- --all

echo done
pause
