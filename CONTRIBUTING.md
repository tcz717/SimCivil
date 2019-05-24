SimCivil 贡献指南
=======

# Git流程

## 基本流程

 1. 发起新的issue或者从backlog里面选一个并assign给自己
 2. 在本地clone `master`分支并创建一个个人分支`xxx-dev` (ex: `tcz-dev`) 
 3. 完成代码编写，存入一个或多个commit
	 - commit 格式
		Add/Change/Remove/Clean/Fix *SOMETHING*
	 - 例子
	   Add a test in unit tests
 4. 根据需要上传个人分支到主仓库 也可不上传
 5. 当全部开发完毕后先**在本地**个人分支上创建一个新分支`CATEGORY/#NUM`
	 可以取的分类有
	 - feature
	 - bug
	 - proposal
	 - doc
	 - test
	 - share
   
	目录内接的数字为issue的编号
	例如 `feature/#12`
6. 执行`pull` 更新master
7. 在新分支上依次执行rebase和squash，使得新分支刚好在最新的`master`分支之后
8. push并在GitHub上创建pull request到master，pull request的标题应当为 Fix/Resolve/Close #NUM，assignee为自己，指定至少一个reviewer
9. 等待code review 通过
10. 通过后执行merge 并删除原有分支

## 原则

 - 只允许在个人分支和`develop`分支上hard push
 - 不准直接修改master
 - 所有pull 必须经过code review
 - 在不用hard push的情况下尽可能的使得提交的commit合并为一条 并使用fast forward合并（提前在本地或个人分支rebase和squash好可以解决大部分问题）
 - 如果需要运行在开发服务器进行测试，可以创建一个带有`[WIP]`的pull request或者提交到`develop`分支上，代码会在[appveyor](https://ci.appveyor.com/project/tcz717/simcivil)上自动编译，然后手动触发部署到开发服务器上

 
