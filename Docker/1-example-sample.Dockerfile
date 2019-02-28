FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY ./out .
ENTRYPOINT ["dotnet", "itworks.dll"]