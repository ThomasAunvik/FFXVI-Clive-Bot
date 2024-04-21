"use client";
import { getCurrentUser } from "@/lib/api/users";
import type { UserModel } from "@/lib/models/user/UserModel";
import Image from "next/image";
import Link from "next/link";
import { useEffect, useReducer, useState } from "react";
import useIsMounted from "../misc/useIsMounted";
import { Skeleton } from "../ui/skeleton";

export const DashboardUser = () => {
  const [user, setUser] = useState<UserModel | null>(null);

  const isMounted = useIsMounted();

  useEffect(() => {
    if (!isMounted) return;
    if (document.cookie.includes(".AspNetCore.Cookies")) {
      getCurrentUser().then((res) => {
        if (!res) return;
        setUser(res);
      });
    }
  }, [isMounted]);

  const image = !user ? (
    <Skeleton className="h-8 w-8 rounded-full" />
  ) : (
    <Image
      className="h-8 w-8 rounded-full"
      src={`https://cdn.discordapp.com/avatars/${user.userId}/${user.avatar}.jpg`}
      width={128}
      height={128}
      alt="user"
    />
  );

  return (
    <div className="ms-3 flex items-center">
      <div className="peer/user">
        <input type="checkbox" className="hidden" id="user-dropdown" />
        <label htmlFor="user-dropdown">
          <div
            className="flex rounded-full bg-gray-800 text-sm dark:focus:ring-gray-600 dark:hover:ring-gray-600 focus:ring-4 focus:ring-gray-300 hover:ring-4 hover:ring-gray-300"
            aria-expanded="false"
            data-dropdown-toggle="dropdown-user"
          >
            <span className="sr-only">Open user menu</span>
            {image}
          </div>
        </label>
      </div>
      <div
        className="margin-0 -translate-x-4 absolute top-0 right-0 bottom-auto left-auto z-50 my-4 hidden translate-y-14 transform list-none divide-y divide-gray-100 rounded bg-white text-base shadow peer-has-[:checked]/user:block dark:divide-gray-600 dark:bg-gray-700"
        id="dropdown-user"
      >
        <div className="px-4 py-3">
          <p className="text-gray-900 text-sm dark:text-white">
            {!user ? "Loading user..." : user.username}
          </p>
          <p className="truncate font-medium text-gray-900 text-sm dark:text-gray-300">
            {!user ? "Loading user..." : user.userId}
          </p>
        </div>
        <ul className="py-1" role="menu">
          <li>
            <Link
              href="/dashboard"
              className="block px-4 py-2 text-gray-700 text-sm dark:hover:bg-gray-600 hover:bg-gray-100 dark:hover:text-white dark:text-gray-300"
              role="menuitem"
            >
              Dashboard
            </Link>
          </li>
          <li>
            <Link
              href="/dashboard/settings"
              className="block px-4 py-2 text-gray-700 text-sm dark:hover:bg-gray-600 hover:bg-gray-100 dark:hover:text-white dark:text-gray-300"
              role="menuitem"
            >
              Settings
            </Link>
          </li>
        </ul>
      </div>
    </div>
  );
};
