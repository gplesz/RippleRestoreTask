RippleRestoreTask
=================

## What is [Ripple](http://darthfubumvc.github.io/ripple/ripple/)? 

> Ripple is a new kind of package manager that was created out of heavy usage of the standard NuGet client. The feeds, the protocol, and the packages are the same. Ripple just embodies differing opinions and provides a new way of consuming them that is friendlier for continuous integration.

The specific functionality we are interested in is [Ripple Restore](http://darthfubumvc.github.io/ripple/ripple/commands/restore/) 

## How to get it

[RippleRestoreTask is hosted on nuget](https://www.nuget.org/packages/RippleRestoreTask/)  

And can eb installed by running the following in the **Package Management Console:**

    PM> Get-Project -All | Install-Package RippleRestoreTask
 

## What does RippleRestoreTask do? 

Ripple work remarkable well at restoring dependencies. So you can avoid checking in your dependencies and then run `ripple.exe restore` before building to restore your dependencies. However it does not handle two problems 

### Providing "Open Solution and run" behaviour  

While you can have a "RunMeFirst.cmd" it is not really ideal. Developers expect to be able to open a Solution and compile.

RippleRestoreTask achieves this by plugging into the build chain and before a build ensuring that your dependencies are up to date through calling `ripple.exe restore`.

### Delivery and update of ripple.exe

RippleRestoreTask is delivered via a nuget package. Since nuget handles delivery and (with nuget 2.7 and up) automatic solution level package restore. This design decision also means we can use nuget to update RippleRestoreTask.

#### I though we were using Ripple for managing packages?

Yes this is correct. However the way nuget adds RippleRestoreTask it will be excluded from Ripple handles packages. 

So the chain is `nuget delivers RippleRestoreTask` > `RippleRestoreTask triggers ripple restore`.

## What is doesn't do

RippleRestoreTask is targeted at providing one specific feature i.e. "Restoring dependencies as part of a build". Id you want to use the other more advanced [commands of Ripple](http://darthfubumvc.github.io/ripple/ripple/commands/) you should call ripple.exe explicitly. This can be done at a machine level by adding Ripple.exe to your Path environment variable  

## Other bits

### Install in every project

The RippleRestoreTask should be installed in every project in the solution. This is due to the lack of a "solution level" build event in MSBuild. 

This can be achieved by running the following in the **Package Management Console:**

    PM> Get-Project -All | Install-Package RippleRestoreTask

### Only runs once per solution

The actual restore only runs once per solution and only on the first build. After this it is assumed you will be adding/removing your dependencies using ripple.exe explicitly. If your some reason you want the RippleRestoreTask to re-run then restart Visual Studio and rebuild.

### Performance overhead

#### Restoring when packages already exist

If all packages a;ready exist the initial restore will take up to 1 second (depending on your hardware) to verify the packages. As noted above this will only occur once for the first solution build.    

#### Restoring when packages don't exist

This is variable based on several factors

 * The number of package dependencies you have
 * How large the packages are
 * How fast you internet connection is
 * If the packages already exist in you machine cache 

## License  

Licensed under [The MIT License](http://opensource.org/licenses/MIT) by [NServiceBus Ltd](http://www.particular.net/)

## Icon

<a href="http://thenounproject.com/noun/cloud-download/#icon-No2786" target="_blank">Cloud Download</a> designed by <a href="http://thenounproject.com/somerandomdude" target="_blank">P.J. Onori</a> from The Noun Project