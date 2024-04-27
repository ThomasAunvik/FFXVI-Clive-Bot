import { NavigationMenuLogin } from "@/components/NavigationMenuLogin";
import {
  NavigationMenu,
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  NavigationMenuTrigger,
} from "@/components/ui/navigation-menu";

const MainPageNavBar = async () => {
  return (
    <div className="absolute flex w-full flex-row justify-between pt-2 pr-4 pl-4">
      <NavigationMenu className="">
        <NavigationMenuList>
          <NavigationMenuItem>
            <NavigationMenuTrigger className="w-60">
              <span className="flex w-full flex-row">Abilities</span>
            </NavigationMenuTrigger>
            <NavigationMenuContent className="h-72 min-w-80 p-4">
              <NavigationMenuLink>Link</NavigationMenuLink>
            </NavigationMenuContent>
          </NavigationMenuItem>
          <NavigationMenuItem>
            <NavigationMenuTrigger>Skills</NavigationMenuTrigger>
            <NavigationMenuContent className="h-72 min-w-80 p-4">
              <NavigationMenuLink>Link</NavigationMenuLink>
            </NavigationMenuContent>
          </NavigationMenuItem>
        </NavigationMenuList>
      </NavigationMenu>
      <NavigationMenuLogin />
    </div>
  );
};

export default MainPageNavBar;
