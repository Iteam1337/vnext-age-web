# vnext-age-web

This is so cool!

Read more about aspnet here: https://github.com/aspnet/home

*Mac OS*

Install the necessary stuff via brew.

`brew tap aspnet/dnx`
`brew update`
`brew install dnvm`

Run.

source dnvm.sh;
dnvm upgrade; # This makes dnu and dnx available in your current shell
dnu restore; # Downloads MyGet (nuget) packages defined in project.json
dnx . kestrel; # Runs the code so that you can access it at http://localhost:5004

*Linux*

Pretty much the same as above, only you need to install mono and dnvm for your disto.

*Docker*

sh deploy.sh # Magic!

*Windows*

Setup

@powershell -NoProfile -ExecutionPolicy unrestricted -Command "&{$Branch='dev';iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/dev/dnvminstall.ps1'))}"

Run

dnx . web
