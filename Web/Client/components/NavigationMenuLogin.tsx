"use client";
import useIsMounted from "@/components/misc/useIsMounted";
import { Button } from "@/components/ui/button";
import {
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuList,
  NavigationMenuRight,
  NavigationMenuTrigger,
} from "@/components/ui/navigation-menu";
import { getCurrentUser } from "@/lib/api/client/users";
import Link from "next/link";
import { useEffect, useState } from "react";

export const NavigationMenuLogin = () => {
  const isMounted = useIsMounted();
  const [isLoggedIn, setLoggedIn] = useState<boolean>(false);

  const [username, setUsername] = useState<string | null>(null);

  useEffect(() => {
    if (!isMounted) return;
    if (document.cookie.includes(".AspNetCore.Cookies")) {
      setLoggedIn(true);

      getCurrentUser().then((res) => {
        if (!res) return;
        setUsername(res.username);
      });
    }
  }, [isMounted]);

  return (
    <NavigationMenuRight className="right-0">
      <NavigationMenuList className="right-0">
        <NavigationMenuItem>
          <NavigationMenuTrigger>
            <span className="flex w-full flex-row">
              {isLoggedIn && !username ? (
                <span>Loading...</span>
              ) : username ? (
                <span className="flex flex-row gap-1">
                  <span>Logged in as</span>
                  <span>{username}</span>
                </span>
              ) : (
                <span>Login</span>
              )}
            </span>
          </NavigationMenuTrigger>
          <NavigationMenuContent className="p-4">
            <div>
              {username ? (
                <div className="flex flex-col gap-2">
                  <Link href="/dashboard">
                    <Button>Dashboard</Button>
                  </Link>
                  <Link href="/signout">
                    <Button className="w-full">Sign out</Button>
                  </Link>
                </div>
              ) : (
                <Link href="/signin">
                  <Button type="submit">Login with Discord</Button>
                </Link>
              )}
            </div>
          </NavigationMenuContent>
        </NavigationMenuItem>
      </NavigationMenuList>
    </NavigationMenuRight>
  );
};
