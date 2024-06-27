FROM mysql:latest as FeirappDB
ENV MYSQL_ROOT_PASSWORD=root
COPY init.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/init.sh
ENTRYPOINT ["init.sh"]