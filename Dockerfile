FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /gspAPI

# Copy everything
COPY gspAPI/ ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as backend
# Set system timezone
RUN rm -f /etc/localtime
RUN ln -sf /usr/share/zoneinfo/posix/Europe/Belgrade /etc/localtime
RUN date
WORKDIR /gspAPI
COPY --from=build-env /gspAPI/out .
EXPOSE 80 443

# Nextjs Frontend
FROM node:18-alpine AS base
WORKDIR /
# Install dependencies only when needed
FROM base AS deps
# Check https://github.com/nodejs/docker-node/tree/b4117f9333da4138b03a546ec926ef50a31506c3#nodealpine to understand why libc6-compat might be needed.
RUN apk add --no-cache libc6-compat

#dotnet dependencies 
WORKDIR /app

# Install dependencies based on the preferred package manager
COPY Frontend/gsp/package.json Frontend/gsp/yarn.lock* Frontend/gsp/package-lock.json* Frontend/gsp/pnpm-lock.yaml* ./
RUN \
	if [ -f yarn.lock ]; then yarn --frozen-lockfile; \
	elif [ -f package-lock.json ]; then npm ci; \
	elif [ -f pnpm-lock.yaml ]; then yarn global add pnpm && pnpm i --frozen-lockfile; \
	else echo "Lockfile not found." && exit 1; \
	fi


# Rebuild the source code only when needed
FROM base AS builder
WORKDIR /app
COPY --from=deps /app/node_modules ./node_modules
COPY Frontend/gsp/ .

# Next.js collects completely anonymous telemetry data about general usage.
# Learn more here: https://nextjs.org/telemetry
# Uncomment the following line in case you want to disable telemetry during the build.
ENV NEXT_TELEMETRY_DISABLED 1

#RUN yarn build

# If using npm comment out above and use below instead
RUN npm run build

# Production image, copy all the files and run next
FROM base AS runner

WORKDIR /app
RUN apk add --no-cache aspnetcore7-runtime
RUN apk add --update --no-cache bash
RUN apk add --no-cache tzdata


ENV NODE_ENV production
ENV NEXT_TELEMETRY_DISABLED 1
ENV TZ Europe/Belgrade

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

COPY --from=builder /app/public ./public

# Set the correct permission for prerender cache
RUN mkdir .next
#RUN chown nextjs:nodejs .next

# Automatically leverage output traces to reduce image size
# https://nextjs.org/docs/advanced-features/output-file-tracing
COPY --from=builder --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=builder --chown=nextjs:nodejs /app/.next/static ./.next/static

#copy backend dotnet
COPY --from=backend ./gspAPI/ /gspAPI/
COPY ./runDocker.sh /


#USER nextjs

EXPOSE 3000

ENV PORT 3000
# set hostname to localhost
#ENV HOSTNAME "localhost"

ENTRYPOINT ["bash", "/runDocker.sh"]

