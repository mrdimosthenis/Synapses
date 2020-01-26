import setuptools

with open("README.md", "r") as fh:
    long_description = fh.read()

setuptools.setup(
    name="SynapsesPy",
    version="0.0.1alpha",
    author="Dimosthenis Michailidis",
    author_email="mrdimosthenis@hotmail.com",
    description="A lightweight Neural Network library, for js, jvm and .net",
    long_description=long_description,
    long_description_content_type="text/markdown",
    url="https://github.com/mrdimosthenis/Synapses",
    packages=setuptools.find_packages(),
    classifiers=[
        "Programming Language :: Python :: 3",
        "License :: OSI Approved :: MIT License",
        "Operating System :: OS Independent",
    ],
    python_requires='>=3.6',
)
