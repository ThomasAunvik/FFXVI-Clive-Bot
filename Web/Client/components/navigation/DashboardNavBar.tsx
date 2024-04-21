"use client";
import { getCurrentUser } from "@/lib/api/users";
import {
  ChevronDown,
  ChevronUp,
  LayoutDashboardIcon,
  LogOutIcon,
  MenuIcon,
  SettingsIcon,
  ShieldIcon,
  SwordIcon,
  UserIcon,
  UsersIcon,
} from "lucide-react";
import Link from "next/link";
import { usePathname } from "next/navigation";
import type React from "react";
import { Suspense, useEffect, useState } from "react";
import useIsMounted from "../misc/useIsMounted";
import { Separator } from "../ui/separator";
import { DashboardUser } from "./DashboardUser";
import { NavEntry } from "./NavEntry";
import type { NavigationEntry } from "./entry";

interface DashboardProps {
  children: React.ReactNode;
}

const redirectToLogin = () => {
  window.location.href = `/signin?redirect=${window.location.pathname}`;
};

const NavList = ({ entries }: { entries: NavigationEntry[] }) => {
  return (
    <ul className="space-y-2 font-medium">
      {entries.map((nav) => (
        <li key={`entry-${nav.name}`}>
          {nav.seperate && (
            <Separator
              orientation="horizontal"
              className="mb-4 h-[1px] bg-gray-700"
            />
          )}
          {nav.children ? (
            <div>
              <div className="peer/nav group/navg">
                <input
                  type="checkbox"
                  id={`navcol-${nav.name}`}
                  className="navcol hidden"
                />
                <label htmlFor={`navcol-${nav.name}`}>
                  <span className="group/box flex items-center rounded-lg p-2 text-gray-900 dark:hover:bg-gray-700 hover:bg-gray-100 dark:text-white">
                    <span className="h-5 w-5 text-gray-500 transition duration-75 dark:group-hover/box:text-white dark:text-gray-400 group-hover/box:text-gray-900">
                      {nav.icon}
                    </span>
                    <span className="ms-3 flex w-full flex-row justify-between">
                      <span>{nav.name}</span>
                      <span>
                        <ChevronDown className="group-has-[.navcol:checked]/navg:rotate-180" />
                      </span>
                    </span>
                  </span>
                </label>
              </div>
              <div className="mt-2 ml-4 max-h-0 transform overflow-clip transition-[max-height] duration-250 ease-in-out peer-has-[.navcol:checked]/nav:max-h-56">
                <NavList entries={nav.children} />
              </div>
            </div>
          ) : (
            <NavEntry entry={nav} />
          )}
        </li>
      ))}
    </ul>
  );
};

const DashboardNavBar = (props: DashboardProps) => {
  const { children } = props;
  const currentPath = usePathname();

  const navigation: NavigationEntry[] = [
    {
      key: "dashboard",
      name: "Dashboard",
      href: "/dashboard",
      icon: <LayoutDashboardIcon />,
    },
    {
      key: "skills",
      name: "Skills",
      href: "/dashboard/skills",
      icon: <SwordIcon />,
    },
    {
      key: "characters",
      name: "Characters",
      href: "/dashboard/characters",
      icon: <UsersIcon />,
    },
    {
      key: "settings",
      name: "Settings",
      icon: <SettingsIcon />,
      children: [
        {
          key: "open-settings",
          name: "Open Settings",
          href: "/dashboard/settings/general",
          icon: <SettingsIcon />,
        },
        {
          key: "moderator",
          name: "Moderators",
          href: "/dashboard/settings/moderator",
          icon: <ShieldIcon />,
        },
      ],
    },
    {
      key: "signout",
      name: "Signout",
      href: "/signout",
      seperate: true,
      icon: <LogOutIcon />,
    },
  ];

  return (
    <div>
      <nav className="peer/topnav fixed top-0 z-50 w-full border-gray-200 border-b bg-white dark:border-gray-700 dark:bg-gray-800">
        <div className="px-3 py-3 lg:px-5 lg:pl-3">
          <div className="flex items-center justify-between">
            <div className="flex items-center justify-start rtl:justify-end">
              <div id="togglebutton">
                <input type="checkbox" id="navbar-toggle" className="hidden" />
                <label htmlFor="navbar-toggle">
                  <div className="inline-flex items-center rounded-lg p-2 text-gray-500 text-sm sm:hidden dark:hover:bg-gray-700 hover:bg-gray-100 dark:text-gray-400 focus:outline-none dark:focus:ring-gray-600 focus:ring-2 focus:ring-gray-200">
                    <span className="sr-only">Open sidebar</span>
                    <MenuIcon aria-hidden="true" />
                  </div>
                </label>
              </div>
              <a href="https://flowbite.com" className="ms-2 flex md:me-24">
                <span className="self-center whitespace-nowrap font-semibold text-xl dark:text-white sm:text-2xl">
                  FFXVI Clive Bot
                </span>
              </a>
            </div>
            <div className="flex items-center">
              <DashboardUser />
            </div>
          </div>
        </div>
      </nav>

      <aside
        id="logo-sidebar"
        className="-translate-x-full fixed top-0 left-0 z-40 h-screen w-64 border-gray-200 border-r bg-white pt-20 transition-transform peer-has-[#navbar-toggle:checked]/topnav:translate-x-0 sm:translate-x-0 dark:border-gray-700 dark:bg-gray-800"
        aria-label="Sidebar"
      >
        <div className="h-full overflow-y-auto bg-white px-3 pb-4 dark:bg-gray-800">
          <NavList entries={navigation} />
        </div>
      </aside>
      <div className="mt-14 p-4 sm:ml-64">{children}</div>
    </div>
  );
};

export default DashboardNavBar;
