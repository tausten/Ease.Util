# Ease.Util (yet another catch-all library)
Welcome to yet another catch-all library. This package serves the following purposes:
* provide a (potentially temporary) home for small bits of code
* allow these to be used without restriction in personal and commercial projects
* act as a sandbox/template for the mechanics of sharing things OSS-wise

As for nuget packaging, initially, there is a single `Ease.Util.nupkg`, but this 
may be broken out into separate specialized packages as time goes on.


## Contributing
If you would like to contribute, and/or have suggestions such as a better OSS project 
home for one or more components you see hanging out in this limbo, please use the 
[Git Issues](https://github.com/tausten/Ease.Util/issues) mechanism to submit feature 
requests / bug reports / re-homing suggestions, etc...  Alternatively, if you've forked
the repo and would like to share some of your work back, please follow the usual
pull request mechanism, and we'll review, comment, and if all goes well, merge your changes
into an upcoming release.

## Documentation
We use [DocFx](https://dotnet.github.io/docfx/) to generate and maintain these pages. When 
adding classes, do add meaningful `///` comments inline to class, method, member, etc... and 
these will automatically be incorporated into the published documentation on the next full
release to the production [NuGet](https://www.nuget.org/packages/Ease.Util/) feed. For documentation 
requiring something more substantial than is comfortable to capture in the `///` comments inline, 
please create a new (or edit an appropriate existing) article under the `docfx_project/articles` 
folder. You are encouraged to use markdown for these articles, but other formats are suppored by 
DocFx if they genuinely are more applicable to your situation.

If you do create a brand new article, please remember to add it to the `docfx_project/articles/toc.yml` 
file or it will likely not be found and enjoyed by folks.

