# Use a lightweight Nginx image as the base
FROM nginx:alpine

# Copy your HTML and JavaScript files into the default web root directory of Nginx
COPY . /usr/share/nginx/html

# Optionally, copy a custom Nginx configuration file if needed
# COPY nginx.conf /etc/nginx/nginx.conf

# Expose port 80 to allow external access
EXPOSE 80

# Start Nginx in the foreground when the container starts
CMD ["nginx", "-g", "daemon off;"]
