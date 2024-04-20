import { NoAuthError } from "@/lib/errors";

const LOGIN_COOKIE_NAME = ".AspNetCore.Cookies";

export const apiFetch = <T>(
  path: string,
  method: "GET" | "POST" | "PUT",
  body?: T,
) => {
  const cookieValue = document.cookie
    .split("; ")
    .find((row) => row.startsWith(`${LOGIN_COOKIE_NAME}=`))
    ?.split("=")[1];

  if (!cookieValue) {
    throw new NoAuthError("User not logged in.");
  }

  return fetch(path, {
    method: method,
    headers: {
      Cookie: `${LOGIN_COOKIE_NAME}=${cookieValue}`,
      "Content-Type": "application/json",
    },
    body: JSON.stringify(body),
  });
};

export const apiGET = async <T>(path: string) => {
  const res = await apiFetch(path, "GET");
  const data = await res.json();
  return data as T;
};

export const apiPOST = async <T, B>(path: string, body: B) => {
  const res = await apiFetch(path, "POST", body);
  const data = await res.json();
  return data as T;
};

export const apiPUT = async <T, B>(path: string, body: B) => {
  const res = await apiFetch(path, "PUT", body);
  const data = await res.json();
  return data as T;
};
