import subprocess
import os

os.chdir('c:\\Users\\Asus\\Desktop\\assignment')

env = os.environ.copy()
env['FILTER_BRANCH_SQUELCH_WARNING'] = '1'
env['GIT_AUTHOR_NAME'] = 'Kaveesha Dissanayake'
env['GIT_AUTHOR_EMAIL'] = '90174106+kaveeshahq@users.noreply.github.com'
env['GIT_COMMITTER_NAME'] = 'Kaveesha Dissanayake'
env['GIT_COMMITTER_EMAIL'] = '90174106+kaveeshahq@users.noreply.github.com'

cmd = [
    'git', 'filter-branch', '-f',
    '--env-filter', 
    'export GIT_AUTHOR_NAME="Kaveesha Dissanayake";'
    'export GIT_AUTHOR_EMAIL="90174106+kaveeshahq@users.noreply.github.com";'
    'export GIT_COMMITTER_NAME="Kaveesha Dissanayake";'
    'export GIT_COMMITTER_EMAIL="90174106+kaveeshahq@users.noreply.github.com";',
    '--', '--all'
]

result = subprocess.run(cmd, env=env, capture_output=True, text=True)
print(result.stdout)
print(result.stderr)
print(f"Return code: {result.returncode}")
