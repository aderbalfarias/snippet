## Git Commands

### Basics

| Command | Description |
| ------- | ----------- |
| `git clone [url]` | Clone a repository |
| `git status` | Check status |
| `git fetch` | Update your remote-tracking branches |
| `git pull` | It does a git fetch followed by a git merge |
| `git add .` | Adding to staging area |
| `git commit -m "message"` | Adding files to local repository |
| `git push origin [branch name]` | It pushes the code to remote branch |
| `git branch` | List branches (the asterisk denotes the current branch) |
| `git branch -a` | List all branches (local and remote) |
| `git branch [branch name]` | Create a new branch |
| `git branch -d [branch name]` | Delete a branch |
| `git push origin --delete [branch name]` | Delete a remote branch |
| `git checkout -b [branch name]` | It creates a new branch |
| `git checkout [branch name]` | Switch branches |
| `git checkout -` | Switch to the branch last checked out |


### Pull Request: (You have done stashes, commits and it's ready to push)

| Command | Description |
| ------- | ----------- |
| `git checkout [branch origin]` | Checkout a branch |
| `git pull` |  Get latest code |
| `git checkout [branch name]` | Branch the code was written |
| `git merge [branch origin]` | Merge branch origin (it may be master into created branch|
| `git push --set-upstream origin [branch name]` | Send a pull request to origin from a branch you have created |