# Umbraco SiteLock

[![NuGet release](https://img.shields.io/nuget/v/Cogworks.SiteLock.svg)](https://www.nuget.org/packages/Cogworks.SiteLock)
[![Our Umbraco project page](https://img.shields.io/badge/our-umbraco-orange.svg)](https://our.umbraco.org/projects/website-utilities/sitelock/)

A security package for Umbraco 7. Lock down an Umbraco website from viewers. Only users who are logged into the backoffice can see the public website.

## Getting started

This package is supported on Umbraco 7.1.2.

### Installation
SiteLock is available from Our Umbraco, NuGet, or as a manual download directly from GitHub.

#### Our Umbraco repository
You can find a downloadable package, along with a discussion forum for this package, on the [Our Umbraco](https://our.umbraco.org/projects/website-utilities/sitelock/) site.

#### NuGet package repository
To [install from NuGet](https://www.nuget.org/packages/Cogworks.SiteLock/), run the following command in your instance of Visual Studio.

    PM> Install-Package Cogworks.SiteLock
	
##How it works
SiteLock is simply a module which is loaded dymically at runtime. It inspects the domain of each request, and then either allows it, or throws a HttpException with a status code of 403.
	
## Usage
After installing the package, you'll be able to lock any website via /config/SiteLock.config.

###Locked Domains
This section of the configuration specifies specific domains to be locked in the Umbraco instance.

Example:
~~~xml
<lockedDomains>
    <domain>localhost</domain>
    <domain>staging.sitelock.local</domain>
</lockedDomains>
~~~

To lock all domains, simply use *.
Example:
~~~xml
<lockedDomains>
    <domain>*</domain>
</lockedDomains>
~~~

###Ignored Paths
This section of the configuration allows you to specify paths which can be ignored. 

Example:
~~~xml
<ignoredPaths>
	<path>/umbraco</path>
	<path>/403.html</path>
</ignoredPaths>
~~~
  
####Important  
Note that the paths are actually wildcards. So in the example above, we ignore paths which "start with" /umbraco. This allows for various routes such as /umbraco/api/myapi/ to pass through. 

Changes to /config/SiteLock.config will require an application restart, as the config file cached at startup.   
	
### Integration
SiteLock was designed to be simple for developers. Just install via nuget or the Umbraco backoffice. Configure /config/SiteLock.config and you're good to go. 


### Contribution guidelines
To raise a new bug, create an issue on the GitHub repository. To fix a bug or add new features, fork the repository and send a pull request with your changes. Feel free to add ideas to the repository's issues list if you would to discuss anything related to the package.

### Who do I talk to?
This project is maintained by [Cogworks](http://www.thecogworks.com/) and contributors. If you have any questions about the project please contact us through the forum on Our Umbraco, on [Twitter](https://twitter.com/cogworks), or by raising an issue on GitHub.

## License
Copyright &copy; 2017 [The Cogworks Ltd](http://www.thecogworks.com/), and other contributors

Licensed under the MIT License.