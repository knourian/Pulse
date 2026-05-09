#!/bin/bash

set -e

SERVICE_NAME=pulse-agent
INSTALL_PATH=/opt/pulse-agent
SERVICE_FILE=/etc/systemd/system/pulse-agent.service


echo "========================================"
echo "Creating install directory"
echo "========================================"

sudo mkdir -p $INSTALL_PATH


echo ""
echo "========================================"
echo "Copying published files"
echo "========================================"

sudo cp -r publish/linux-x64/* $INSTALL_PATH/


echo ""
echo "========================================"
echo "Installing systemd service"
echo "========================================"

sudo cp pulse-agent.service $SERVICE_FILE


echo ""
echo "========================================"
echo "Reloading systemd"
echo "========================================"

sudo systemctl daemon-reload


echo ""
echo "========================================"
echo "Enabling service"
echo "========================================"

sudo systemctl enable $SERVICE_NAME


echo ""
echo "========================================"
echo "Starting service"
echo "========================================"

sudo systemctl restart $SERVICE_NAME


echo ""
echo "========================================"
echo "Service installed successfully"
echo "========================================"

sudo systemctl status $SERVICE_NAME