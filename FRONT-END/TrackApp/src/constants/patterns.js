export const PHONE_NUMBER = /^\+?\d{10,14}$/;
export const DEVICE_PASSWORD = /^[a-zA-Z0-9]{6,}$/;
export const NUMBER = /^\d+$/;
export const CONTACT_NAME = /^[a-zA-Z0-9\s]+$/;
export const IP = /^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$/;
export const HOST_NAME = /^([a-z0-9A-Z]\.)*[a-z0-9-]+\.([a-z0-9]{2,24})+(\.co\.([a-z0-9]{2,24})|\.([a-z0-9]{2,24}))*$/;