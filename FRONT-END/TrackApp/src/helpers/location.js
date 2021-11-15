export default class Location {
    constructor(lat, lng, alt, speed, dir, battery) {
        this.latitude = Number(lat);
        this.longitude = Number(lng);
        this.altitude = Number(alt);
        this.speed = Number(speed);
        this.direction = Number(dir);
        this.battery = Number(battery);
    }

    static toArray(loc) {
        return [
            loc.latitude,
            loc.longitude,
            loc.altitude,
            loc.speed,
            loc.direction,
            loc.battery
        ];
    }
}