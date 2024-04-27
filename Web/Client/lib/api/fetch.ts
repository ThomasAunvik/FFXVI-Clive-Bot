import { cookies } from "next/headers";
import "server-only";

const getServerApiUrl = () => {
	const env = process.env.SERVER_API_URL;
	if (!env || env === "") {
		throw new Error("SERVER_API_URL env is not set");
	}
	return env;
};

const LOGIN_COOKIE_NAME = ".AspNetCore.Cookies";

export const apiFetch = async <T>(
	path: string,
	method: "GET" | "POST" | "PUT" | "DELETE",
	body?: T,
) => {
	const cook = cookies();
	const cookieValue = cook.get(LOGIN_COOKIE_NAME);
	const loginValue = cookieValue?.value;

	const totalPath = `${getServerApiUrl()}${path}`;

	const res = await fetch(totalPath, {
		method: method,
		headers: {
			...(loginValue ? { Cookie: `${LOGIN_COOKIE_NAME}=${loginValue}` } : {}),
			"Content-Type": "application/json",
		},
		body: JSON.stringify(body),
		credentials: "include",
	});

	if (res.status !== 200) {
		throw Error(
			`Failed to send API request (${res.status}): ${res.statusText}`,
		);
	}

	return res;
};

export const apiGET = async <T>(path: string) => {
	const res = await apiFetch(path, "GET");
	const text = await res.text();
	const data = JSON.parse(text);
	return data as T;
};

// biome-ignore lint/suspicious/noExplicitAny: body can be any
export const apiPOST = async <T>(path: string, body: any) => {
	const res = await apiFetch(path, "POST", body);
	const data = await res.json();
	return data as T;
};

// biome-ignore lint/suspicious/noExplicitAny: body can be any
export const apiPUT = async <T>(path: string, body: any) => {
	const res = await apiFetch(path, "PUT", body);
	const data = await res.json();
	return data as T;
};

export const apiDELETE = async <T>(path: string) => {
	const res = await apiFetch(path, "DELETE");
	const data = await res.json();
	return data as T;
};
