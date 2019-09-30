# VLSM Calculator
## Installation instructions
 *  Find the [Setup.zip](https://gitlab.gitgudaim.com/w200338/vlsm-calc/blob/master/VLSM%20Calc%20Setup.zip) file in te repository and download it
 * Extract/open the zip
 * Open Setup.exe
 * Follow the setup by clicking a combination of **install**, **next** and **finish**
 * Some anti-virus programs block or capture the program when it runs for the first time, just let it do its thing and wait a little bit
## General instructions
Type the address of the network you want to split (e.g. 192.168.1.0) into the first box. Then type your subnetmask in the second box, either in the same notation as the network address (e.g. 255.255.255.0) or cidr (e.g. /24).
For every subnet you want to add you can type the minimum required amount in the third box and then click the add button next to it to add it to the list. To one of these requested subnets just select it in the box and click the remove button next to the list.
When you've added all your subnets you can click the calculate button to the right of your list of requested subnets, the program will then quickly attempt to create a setup that meets your requirements. If the setup is invalid, you requested to many hosts or the combination of hosts isn't possible a little warning will be displayed prompting you take another look at your setup. 
If it does succeed a new list will be created and displayed on the right side of the window. Each line will display the network address and subnet mask (in both notations) of a subnet. Selecting one of these subnets and clicking the details button in the middle (or double clicking the subnet) will bring up a new window with more details about the subnet. You can also put in an ip address at the bottom and click the calculate button to see if the ip address you put in is part of the selected subnet.