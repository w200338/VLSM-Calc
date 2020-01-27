# VLSM Calculator
## Installation instructions
### Application
 * Find the [Setup.zip](https://gitlab.gitgudaim.com/w200338/vlsm-calc/blob/master/VLSM%20Calc%20Setup.zip) file in the repository and download it
 * Extract/open the zip
 * Open Setup.exe
 * Follow the setup by clicking a combination of **install**, **next** and **finish**
 * Some anti-virus programs block or capture the program when it runs for the first time, just let it do its thing and wait a little bit
 
### Standalone .exe
 * Find the [VLSM Calc.exe](https://gitlab.gitgudaim.com/w200338/vlsm-calc/blob/master/VLSM%20Calc.exe) file in the repository and download it
 * When running it for the first time a pop-up will appear about an unknown publisher, click **More information** and then **Execute**

## General instructions
Type the address of the network you want to split (e.g. 192.168.1.0) into the 
first box. Then type your subnetmask in the second box, either in the same 
notation as the network address (e.g. 255.255.255.0) or cidr (e.g. /24).

For every subnet you want to add you can type the minimum required amount in the 
box labeld hosts and then click the **Add** button next to it to add it to the list. 
To remove one of these requested subnets select it and click the **Remove** button 
next to the list. To edit the requested size or give a name to the subnet double 
click it in the list and edit them in the window which opens up.

When you've added all your subnets you can click the **Calculate** button to the right 
of your list of requested subnets, the program will then quickly attempt to create a 
setup that meets your requirements. If the setup is invalid, you requested to many 
hosts or the combination of hosts isn't possible a little warning will be displayed 
prompting you take another look at your setup. 

If it does succeed a new list will be created and displayed on the right side of 
the window. Each line will display the network address and subnet mask (in both 
notations) of a subnet. Selecting one of these subnets and clicking the **Details** 
button in the middle (or double clicking the subnet) will bring up a new window 
with more details about the subnet. You can also put in an ip address at the bottom 
and click the **Calculate** button to see if the ip address you put in is part 
of the selected subnet. At the very bottom of the window there are two arrow button,
these can be used to quickly switch between the generated subnets (in the order 
shown in the main window).

### Subnet mode
By default the program is in VLSM mode, you can change over to subnet mode by 
clicking the **Mode** button and then selecting **Subnet**. This will change some 
labels and disable the remove button. To use this mode simply fill in the amount
of subnets you want ([must be a power of 2](https://en.wikipedia.org/wiki/Power_of_two))
in the box labeled **Subnets** and click on the **Divide** button. The list of 
requests will be automatically filled with the given amount of subnets, you can 
still change their name. Afterwards you can click the **Calculate** button to let
the program calculate the subnets and show them in a list on the right side 
of the window like in VLSM mode.

## Tools
### Subnet size calculator
Calculates the amount of hosts available in a given subnet mask or cidr notation.

### Subnet converter
Let's you convert (valid) subnet masks to their cidr notation and 
vice versa.

### Wildcard mask converter
Let's you convert a subnet mask or cidr notation to its wildcard mask and vice versa.