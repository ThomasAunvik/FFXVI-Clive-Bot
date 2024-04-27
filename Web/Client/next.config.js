/** @type {import('next').NextConfig} */
const nextConfig = {
	output: "standalone",
	reactStrictMode: true,
	images: {
		unoptimized: true,
	},
	experimental: {
		ppr: true,
	},
};

module.exports = nextConfig;
