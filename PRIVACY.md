# Privacy and Personal Data

NuciText.Grammar is a lightweight, dependency-free .NET library for rule-based grammar and text normalisation. It operates entirely in-process on text provided by the calling application and does not collect, transmit, store, or process any personal data on its own behalf.

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Self-Hosted Deployments](#self-hosted-deployments)
- [Data We Handle](#data-we-handle)
  - [Data Provided to the Application](#data-provided-to-the-application)
  - [Data Generated or Collected by the Application](#data-generated-or-collected-by-the-application)
  - [Data Received from Integrations](#data-received-from-integrations)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [Data Protection and Security](#data-protection-and-security)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how NuciText.Grammar at https://github.com/hmlendea/nucitext.grammar handles personal data. It covers the application behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 🏠 Self-Hosted Deployments

NuciText.Grammar is a .NET library distributed via NuGet. It runs entirely within the host application's process. The library itself does not operate as a service, collect telemetry, or communicate with any external endpoint. The host application (the instance operator) controls all configuration, local storage, logs, backups, access controls, retention, and request handling. No data is sent from the library to project maintainers or external services.

## 📥 Data We Handle

### Data Provided to the Application

The library processes only the text strings explicitly passed to its `Correct` method by the calling application. It does not request, collect, or store any personal data on its own.

### Data Generated or Collected by the Application

No personal data is generated or collected automatically by the library. It performs deterministic text transformations and returns the corrected string to the caller.

### Data Received from Integrations

No personal data is received from integrations or third parties. The library has no built-in integrations.

## 🧭 Processing and Use

The application processes the data described above for these verified functions:
- Text correction via `GrammarCorrector.Correct` — Input text string provided by the caller

## 🗄️ Storage, Retention, and Deletion

The library does not persist any data. All processing occurs in memory within the caller's process. The caller controls the lifetime of input and output strings. For self-hosted deployments, the instance operator (the host application) controls all storage, deletion, and backups of any data it passes to or receives from the library.

## 🔗 External Processing and Integrations

The application has no built-in external data transfer, integrations, or third-party services.

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| None | N/A | N/A | N/A |

## 🛡️ Data Protection and Security

The library performs in-memory string transformations using deterministic rules. It does not access the file system, network, or external resources. It does not log, cache, or retain input or output text. For self-hosted deployments, the instance operator is responsible for updates, secrets management, access controls, backups, network exposure, and log protection in the host application.

The original maintainers are not responsible for changes, modifications, or privacy issues introduced by downstream forks.

## 🔄 Document Changes

Update this document when application data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/nucitext.grammar/blob/main/PRIVACY.md.

## 📬 Contact

For questions about application data handling, contact the project maintainers via GitHub issues at https://github.com/hmlendea/nucitext.grammar/issues. For a self-hosted instance, contact the instance operator (the host application maintainer), unless the project explicitly handles the request. Include the library version and a description of the data flow in question; do not send passwords, access tokens, or other secrets.