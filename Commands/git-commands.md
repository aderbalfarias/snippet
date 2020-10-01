## Git Commands

### Basics

| Command | Description |
| ------- | ----------- |
| `git clone [url]` | Clone a repository |
| `git status` | Check status |
| `git fetch` | Update your remote-tracking branches |
| `git pull` | It does a git fetch followed by a git merge |
| `git add --all` | Adding all changes to staging area |
| `git commit -m "message"` | Adding files to local repository |
| `git push origin [branch name]` | It pushes the code to remote branch |
| `git branch` | List branches (the asterisk denotes the current branch) |
| `git branch -a` | List all branches (local and remote) |
| `git branch [branch name]` | Create a new branch |
| `git branch -d [branch name]` | Delete local branch |
| `git push -d [remote name] [branch name]` | Delete remote branch, note: remote name in most of the case is **origin** |
| `git branch -m [old name] [new name]` | rename local branch |
| `git branch -m [new name]` | rename current local branch |
| `git push origin :[old name] [new name]` | delete the old name remote branch and push the new name local branch |
| `git push origin -u [new name]` | reset the upstream branch for the new name local branch |
| `git push origin --delete [branch name]` | Delete a remote branch |
| `git checkout -b [branch name]` | It creates a new branch |
| `git checkout -b [branch name] [commit id]` | It creates a new branch from a specif commit |
| `git checkout [branch name]` | Switch branches |
| `git checkout -` | Switch to the branch last checked out |
| `git checkout -- [file-name.txt]` | Discard changes to a file |
| `git add -A` | Add all new and changed files to the staging area |
| `git rm -r [file-name]` | Remove a file from staging area |
| `git stash save "message"` | It creates a stash that allows we pull code and then merge the code without commit it, like config files |
| `git stash apply stash@{[stash number]}` | Get back the files stashed |
| `git stash pop stash@{[stash number]}` | Return your repository to status before the stash (you will lose changes) |
| `git stash drop stash@{[stash number]}` | Delete stash |
| `git stash apply $stash_rash` | Get back the files from a deleted stash |
| `git stash save --keep-index "message"` | Stashing files that are not in the staging area |
| `git stash show` | show files that changed |
| `git stash show -p` | show the full diff in the files that changed |
| `git reset HEAD filename/*/.` | Removing files from staging area and back to current working directory |
| `git reset --hard HEAD~1` | Removing a commit (You will lose the changes in that commit) |
| `git reset --soft HEAD^` | It will reset the index to HEAD^ (previous commit), however, it will leave the changes in stage area |
| `git reset --hard HEAD^` | rollback changes |
| `git commit --amend -m` | Update message of the last commit |
| `git push --force` | In case you have already pushed code you reset to remote branch |
| `git log --autor=[your name]` | To see your commits |
| `git log` | To list all commits |
| `git log --oneline` | To list all commits in one line |
| `git gc --prune=now` | Way to delete data that has accumulated in Git but is not being referenced by anything |
| `git remote prune origin` | Way to delete data that has accumulated in Git but is not being referenced by anything |
| `git checkout [commit id]` | it goes back back to its commit temporarily |
| `git mv [old name] [new name]` | Basic rename (for case sensitive it has to be different, e.g. in this file) |
| `git merge [branch name]` | Merge items from a branch into your branch |
| `git merge origin/[branch name]` | Merge items from a remote branch into your local branch |
| `git grep "[your search]"` | Search the working directory for anything you need |
| `git commit --amend -m "[new commit message]"` | Amending the most recent commit message (you need to force (-f) the next push if it is on remote already) |
| `git cherry-pick [commit id]` | Copying a commit from one branch to another
| `:wq` | Exit merge screen in git bash |


### Git stash per hunk 

| Command |
| ------- | 
|`git stash save -p "message"` |
|Stashing changes per hunk (if you change 5 peaces in a file it will show a dialog to you choose) |
|Dialog: Stash this hunk [y,n,q,a,d,e,?]? |
|y - stash this hunk |
|n - do not stash this hunk |
|q - quit; do not stash this hunk or any of the remaining ones |
|a - stash this hunk and all later hunks in this file |
|d - do not stash this hunk or any of the later hunks in the file |
|e - manually edit the current hunk |
|? - print help |


### Pull Request: You have done stashes, commits and it's ready to push

| Command | Description |
| ------- | ----------- |
| `git checkout [branch origin]` | Checkout a branch |
| `git pull` |  Get latest code |
| `git checkout [branch name]` | Branch the code was written |
| `git merge [branch origin]` | Merge branch origin (it may be master into created branch|
| `git push --set-upstream origin [branch name]` | Send a pull request to origin from a branch you have created |
|`:wq` | Exit merge screen in git bash |


### Git Sync Fork Example 1: 

| Command | 
| ------- |
| `git clone https://github.com/[Your Git Username]/[Repository in your git].git` |
| `git checkout master` |
| `git remote add upstream git://github.com/[Original Git Username]/[Repository you forked from].git` |
| `git fetch upstream` |
| `git pull upstream master` |
| `git push` |


### Git Sync Fork Example 2: 

| Command | 
| ------- |
| `git checkout master` |
| `git remote add upstream git://github.com/[Original Git Username]/[Repository you forked from].git` |
| `git fetch upstream` |
| `git merge upstream/master` |
| `git push` |


### Git Case Sensitive Rename: 

| Command | 
| ------- |
| `git mv casesensitive temp` |
| `git mv temp CaseSensitive` |
