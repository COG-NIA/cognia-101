(() => {
    const storageKey = "cognia.dailyReminder";
    let reminderTimer;

    function isValidTime(value) {
        if (typeof value !== "string" || !/^\d{2}:\d{2}$/.test(value)) {
            return false;
        }

        const [hours, minutes] = value.split(":").map(Number);
        return hours >= 0 && hours <= 23 && minutes >= 0 && minutes <= 59;
    }

    function clearReminderTimer() {
        if (reminderTimer) {
            window.clearTimeout(reminderTimer);
            reminderTimer = undefined;
        }
    }

    function scheduleReminder() {
        clearReminderTimer();

        if (typeof Notification === "undefined" || Notification.permission !== "granted") {
            return;
        }

        let preferences;
        try {
            preferences = JSON.parse(window.localStorage.getItem(storageKey) || "null");
        } catch {
            return;
        }

        if (!preferences || !isValidTime(preferences.time)) {
            return;
        }

        const [hours, minutes] = preferences.time.split(":").map(Number);
        const nextReminder = new Date();
        nextReminder.setHours(hours, minutes, 0, 0);
        if (nextReminder.getTime() <= Date.now()) {
            nextReminder.setDate(nextReminder.getDate() + 1);
        }

        reminderTimer = window.setTimeout(() => {
            if (Notification.permission === "granted") {
                new Notification("A moment for yourself", {
                    body: "Take a gentle pause and check in with how you are feeling.",
                    tag: "cognia-daily-check-in"
                });
            }
            scheduleReminder();
        }, nextReminder.getTime() - Date.now());
    }

    async function enable(time) {
        if (typeof Notification === "undefined") {
            return "unsupported";
        }

        if (!isValidTime(time)) {
            return "invalid";
        }

        const permission = Notification.permission === "granted"
            ? "granted"
            : await Notification.requestPermission();

        if (permission !== "granted") {
            return permission;
        }

        window.localStorage.setItem(storageKey, JSON.stringify({ time }));
        scheduleReminder();
        return "granted";
    }

    function disable() {
        window.localStorage.removeItem(storageKey);
        clearReminderTimer();
    }

    function notifyCheckInSaved() {
        if (typeof Notification !== "undefined" && Notification.permission === "granted") {
            new Notification("Check-in noted", {
                body: "You took a moment to check in with yourself.",
                tag: "cognia-check-in-saved"
            });
        }
    }

    window.cogniaReminders = { enable, disable, notifyCheckInSaved };
    scheduleReminder();
})();
