IODataBlock
===========

This is the PublishedPackages directory.

## After every update to the .nuproj Nuget package projects copy build output to this folder!

#### Then when someone pulls down the latest and greatest from GitHub they can copy updates to their local nuget package source directory...

#### You can add a local package source under settings in the Nuget Package Manager of VS. Don't forget to change package source when looking for local packages.


#### To include folders like this in your GitHub repository you should modify your .gitignore file like so (make sure to specify the correct path!):

```
# Enable "build/" folder in the NuGet Packages folder since NuGet packages use it for MSBuild targets
!packages/*/build/
!*/PublishedPackages/*
```

or alternately you can do something like so (make sure to specify the correct path!):

#### Use following GitShell commands to add new files in this folder to repository:

```

    cd nuget

    cd release

    git add . -f
```