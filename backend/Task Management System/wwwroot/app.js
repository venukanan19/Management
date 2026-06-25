/* =========================================================
   Task Management System — app.js
   Plain JavaScript using fetch() and .then()
   ========================================================= */

// Base API URLs - change if your controller routes are different
const TASK_API = "/api/Task";
const USER_API = "/api/Users";

// ---------------------------------------------------------
// DASHBOARD PAGE
// ---------------------------------------------------------
function loadDashboard() {
    fetch(TASK_API + "/all")
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            const tasks = result.data;

            let todoCount = 0;
            let progressCount = 0;
            let doneCount = 0;

            for (let i = 0; i < tasks.length; i++) {
                if (tasks[i].status === "Todo") {
                    todoCount++;
                } else if (tasks[i].status === "In Progress") {
                    progressCount++;
                } else if (tasks[i].status === "Done") {
                    doneCount++;
                }
            }

            document.getElementById("totalCount").textContent = tasks.length;
            document.getElementById("todoCount").textContent = todoCount;
            document.getElementById("progressCount").textContent = progressCount;
            document.getElementById("doneCount").textContent = doneCount;
        })
        .catch(function (error) {
            alert("Could not load dashboard data.");
        });
}

// ---------------------------------------------------------
// TASKS PAGE
// ---------------------------------------------------------
function loadTasks() {
    fetch(TASK_API + "/all")
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            displayTasks(result.data);
        })
        .catch(function (error) {
            alert("Could not load tasks.");
        });
}

function displayTasks(tasks) {
    const container = document.getElementById("taskList");

    if (tasks.length === 0) {
        container.innerHTML = "<p>No tasks found.</p>";
        return;
    }

    let html = "";
    for (let i = 0; i < tasks.length; i++) {
        const task = tasks[i];
        html += "<div class='task-card'>";
        html += "<h3>" + task.title + "</h3>";
        html += "<p>" + (task.description || "") + "</p>";
        html += "<p>Assigned to: " + (task.userName || "Unassigned") + "</p>";
        html += "<p>Status: <span class='status-pill'>" + task.status + "</span></p>";
        html += "<select onchange=\"updateStatus(" + task.taskId + ", this.value)\">";
        html += "<option value='Todo'" + (task.status === "Todo" ? " selected" : "") + ">Todo</option>";
        html += "<option value='In Progress'" + (task.status === "In Progress" ? " selected" : "") + ">In Progress</option>";
        html += "<option value='Done'" + (task.status === "Done" ? " selected" : "") + ">Done</option>";
        html += "</select> ";
        html += "<a href='edit-task.html?id=" + task.taskId + "'>Edit</a> ";
        html += "<button onclick='removeTask(" + task.taskId + ")'>Delete</button>";
        html += "</div>";
    }

    container.innerHTML = html;
}

function searchTasks() {
    const keyword = document.getElementById("searchBox").value;

    if (keyword.trim() === "") {
        loadTasks();
        return;
    }

    fetch(TASK_API + "/search?keyword=" + keyword)
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            displayTasks(result.data);
        })
        .catch(function (error) {
            alert("Search failed.");
        });
}

function updateStatus(taskId, newStatus) {
    fetch(TASK_API + "/changestatus/" + taskId, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ status: newStatus }),
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            alert("Status updated.");
            loadTasks();
        })
        .catch(function (error) {
            alert("Could not update status.");
        });
}

function removeTask(taskId) {
    const sure = confirm("Are you sure you want to delete this task?");
    if (!sure) return;

    fetch(TASK_API + "/" + taskId, { method: "DELETE" })
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            alert("Task deleted.");
            loadTasks();
        })
        .catch(function (error) {
            alert("Could not delete task.");
        });
}

// ---------------------------------------------------------
// ADD TASK PAGE
// ---------------------------------------------------------
function loadUsersForDropdown() {
    fetch(USER_API + "/all")
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            const users = result.data;
            const select = document.getElementById("userId");

            let html = "<option value=''>-- Select a user --</option>";
            for (let i = 0; i < users.length; i++) {
                html += "<option value='" + users[i].userId + "'>" + users[i].userName + "</option>";
            }
            select.innerHTML = html;
        })
        .catch(function (error) {
            alert("Could not load users.");
        });
}

function submitAddTask(event) {
    event.preventDefault();

    const title = document.getElementById("title").value;
    const description = document.getElementById("description").value;
    const status = document.getElementById("status").value;
    const userId = document.getElementById("userId").value;

    if (title.trim() === "") {
        alert("Title is required.");
        return;
    }

    if (userId === "") {
        alert("Please select a user.");
        return;
    }

    const taskData = {
        title: title,
        description: description,
        status: status,
        userId: parseInt(userId),
    };

    fetch(TASK_API + "/add", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(taskData),
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            alert("Task created successfully.");
            window.location.href = "tasks.html";
        })
        .catch(function (error) {
            alert("Could not create task.");
        });
}

// ---------------------------------------------------------
// EDIT TASK PAGE
// ---------------------------------------------------------
function loadTaskForEdit() {
    const params = new URLSearchParams(window.location.search);
    const taskId = params.get("id");

    if (!taskId) {
        alert("No task selected.");
        return;
    }

    document.getElementById("taskId").value = taskId;

    fetch(TASK_API + "/" + taskId)
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            const task = result.data;
            document.getElementById("title").value = task.title;
            document.getElementById("description").value = task.description || "";
            document.getElementById("status").value = task.status;

            return fetch(USER_API + "/all");
        })
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            const users = result.data;
            const select = document.getElementById("userId");

            let html = "";
            for (let i = 0; i < users.length; i++) {
                html += "<option value='" + users[i].userId + "'>" + users[i].userName + "</option>";
            }
            select.innerHTML = html;
        })
        .catch(function (error) {
            alert("Could not load task.");
        });
}

function submitEditTask(event) {
    event.preventDefault();

    const taskId = document.getElementById("taskId").value;
    const title = document.getElementById("title").value;
    const description = document.getElementById("description").value;
    const status = document.getElementById("status").value;
    const userId = document.getElementById("userId").value;

    if (title.trim() === "") {
        alert("Title is required.");
        return;
    }

    const taskData = {
        title: title,
        description: description,
        status: status,
        userId: parseInt(userId),
    };

    fetch(TASK_API + "/update/" + taskId, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(taskData),
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            alert("Task updated successfully.");
            window.location.href = "tasks.html";
        })
        .catch(function (error) {
            alert("Could not update task.");
        });
}

// ---------------------------------------------------------
// USERS PAGE
// ---------------------------------------------------------
function loadUsers() {
    fetch(USER_API + "/all")
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            displayUsers(result.data);
        })
        .catch(function (error) {
            alert("Could not load users.");
        });
}

function displayUsers(users) {
    const container = document.getElementById("userList");

    if (users.length === 0) {
        container.innerHTML = "<p>No users found.</p>";
        return;
    }

    let html = "<table border='1' cellpadding='8'>";
    html += "<tr><th>Name</th><th>Email</th><th></th><th></th></tr>";

    for (let i = 0; i < users.length; i++) {
        const u = users[i];
        html += "<tr>";
        html += "<td>" + u.userName + "</td>";
        html += "<td>" + u.email + "</td>";
        html += "<td><a href='user-detail.html?id=" + u.userId + "'>View Tasks</a></td>";
        html += "<td><button onclick='removeUser(" + u.userId + ")'>Delete</button></td>";
        html += "</tr>";
    }

    html += "</table>";
    container.innerHTML = html;
}

function submitAddUser(event) {
    event.preventDefault();

    const userName = document.getElementById("userName").value;
    const email = document.getElementById("email").value;

    if (userName.trim() === "") {
        alert("Name is required.");
        return;
    }

    if (email.trim() === "") {
        alert("Email is required.");
        return;
    }

    const userData = {
        userName: userName,
        email: email,
    };

    fetch(USER_API + "/add", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(userData),
    })
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            alert("User added successfully.");
            document.getElementById("addUserForm").reset();
            loadUsers();
        })
        .catch(function (error) {
            alert("Could not add user.");
        });
}

function removeUser(userId) {
    const sure = confirm("Are you sure you want to delete this user?");
    if (!sure) return;

    fetch(USER_API + "/" + userId, { method: "DELETE" })
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            alert("User deleted.");
            loadUsers();
        })
        .catch(function (error) {
            alert("Could not delete user.");
        });
}

// ---------------------------------------------------------
// USER DETAIL PAGE
// ---------------------------------------------------------
function loadUserDetail() {
    const params = new URLSearchParams(window.location.search);
    const userId = params.get("id");

    if (!userId) {
        alert("No user selected.");
        return;
    }

    fetch(USER_API + "/" + userId + "/tasks")
        .then(function (response) {
            return response.json();
        })
        .then(function (result) {
            const user = result.data;
            document.getElementById("userName").textContent = user.userName;
            document.getElementById("userEmail").textContent = user.email;
            displayUserTasks(user.tasks);
        })
        .catch(function (error) {
            alert("Could not load user.");
        });
}

function displayUserTasks(tasks) {
    const container = document.getElementById("taskList");

    if (!tasks || tasks.length === 0) {
        container.innerHTML = "<p>No tasks assigned to this user.</p>";
        return;
    }

    let html = "";
    for (let i = 0; i < tasks.length; i++) {
        const task = tasks[i];
        html += "<div class='task-card'>";
        html += "<h3>" + task.title + "</h3>";
        html += "<p>" + (task.description || "") + "</p>";
        html += "<p>Status: <span class='status-pill'>" + task.status + "</span></p>";
        html += "<a href='edit-task.html?id=" + task.taskId + "'>Edit</a>";
        html += "</div>";
    }

    container.innerHTML = html;
}