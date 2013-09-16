RippleRestoreTask
=================

![](https://raw.github.com/Particular/RippleRestoreTask/master/Icons/package_icon.png)

## What does RippleRestoreTask do? 

### Firstly what is [Ripple](http://darthfubumvc.github.io/ripple/ripple/)? 

> Ripple is a new kind of package manager that was created out of heavy usage of the standard NuGet client. The feeds, the protocol, and the packages are the same. Ripple just embodies differing opinions and provides a new way of consuming them that is friendlier for continuous integration.

The specific functionality we are interested in is [Ripple Restore](http://darthfubumvc.github.io/ripple/ripple/commands/restore/) 

Ripple work remarkable well at restoring dependencies. So you can avoid checking in your dependencies and then run `ripple.exe restore` before building to restore your dependencies. However it does not handle two problems 

### Providing "Open Solution and run" behaviour  

While you can have a "RunMeFirst.cmd" it is not really ideal. Developers expect to be able to open a Solution and compile.

RippleRestoreTask achieves this by plugging into the build chain and before a build ensuring that your dependencies are up to date through calling `ripple.exe restore`.

## What is doesn't do

RippleRestoreTask is targeted at providing one specific feature i.e. "Restoring dependencies as part of a build". If you want to use the other more advanced [commands of Ripple](http://darthfubumvc.github.io/ripple/ripple/commands/) you should call ripple.exe explicitly. This can be done at a machine level by adding Ripple.exe to your Path environment variable  

## Other bits


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