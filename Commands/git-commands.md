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
| `git checkout -- [file-name.txt]` | Discard changes to a file |
| `git add -A` | Add all new and changed files to the staging area |
| `git rm -r [file-name]` | Remove a file from staging area |
| `git stash save "message"` | It creates a stash that allows we pull code and then merge the code without commit it, like config files |
| `git stash apply stash@{numberOfStash}` | Get back the files stashed |
| `git stash pop stash@{numberOfStash}` | Return your repository to status before the stash (you will lose changes) |
| `git stash drop stash@{numberOfStash}` | Delete stash |
| `git stash apply $stash_rash` | Get back the files from a deleted stash |
| `git stash save --keep-index "message"` | Stashing files that are not in the staging area |
| `git stash show` | show files that changed |
| `git stash show -p` | show the full diff in the files that changed |
| `git reset HEAD filename/*/.` | Removing files from staging area and back to current working directory |
| `git reset --hard HEAD~1` | Removing a commit (You will lose the changes in that commit) |
| `git reset --soft HEAD^` | It will reset the index to HEAD^ (previous commit), however, it will leave the changes in stage area |
| `git reset --hard HEAD^` | rollback changes |
| `git push --force` | In case you have already pushed code you reset to remote branch |
| `git log --autor=[yourName]` | To see your commits |
| `git gc --prune=now` | Way to delete data that has accumulated in Git but is not being referenced by anything |
| `git remote prune origin` | Way to delete data that has accumulated in Git but is not being referenced by anything |
| `:wq` | Exit merge screen in git bash |


### Pull Request: (You have done stashes, commits and it's ready to push)

| Command | Description |
| ------- | ----------- |
| `git checkout [branch origin]` | Checkout a branch |
| `git pull` |  Get latest code |
| `git checkout [branch name]` | Branch the code was written |
| `git merge [branch origin]` | Merge branch origin (it may be master into created branch|
| `git push --set-upstream origin [branch name]` | Send a pull request to origin from a branch you have created |
|`:wq` | Exit merge screen in git bash |


###Git Sync Fork Example 1: 

| Command | 
| `git checkout master` |
| `git remote add upstream git://github.com/REPOSITORY_ORIGIN/PROJECT_ORIGIN` |
| `git fetch upstream` |
| `git merge upstream/master` |
| `git push` |