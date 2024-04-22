import { NoAuthError } from "@/lib/errors";

const API_URL = process.env.NEXT_PUBLIC_API_URL;

const LOGIN_COOKIE_NAME = ".AspNetCore.Cookies";

export const apiClientFetch = <T>(
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

	return fetch(`${API_URL}${path}`, {
		method: method,
		headers: {
			Cookie: `${LOGIN_COOKIE_NAME}=${cookieValue}`,
			"Content-Type": "application/json",
		},
		body: JSON.stringify(body),
	});
};

export const apiClientGET = async <T>(path: string) => {
	const res = await apiClientFetch(path, "GET");
	const data = await res.json();
	return data as T;
};

export const apiClientPOST = async <T, B>(path: string, body: B) => {
	const res = await apiClientFetch(path, "POST", body);
	const data = await res.json();
	return data as T;
};

export const apiClientPUT = async <T, B>(path: string, body: B) => {
	const res = await apiClientFetch(path, "PUT", body);
	const data = await res.json();
	return data as T;
};
