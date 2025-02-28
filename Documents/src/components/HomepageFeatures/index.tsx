import type {ReactNode} from 'react';
import clsx from 'clsx';
import Heading from '@theme/Heading';
import styles from './styles.module.css';

type FeatureItem = {
  title: string;
  emoji: string;
  description: ReactNode;
};

const FeatureList: FeatureItem[] = [
  {
    title: 'Ready to Use',
    emoji: '⚡️',
    description: (
      <>
        You can use it immediately by installing it into your project.
      </>
    ),
  },
  {
    title: 'Easy to Use',
    emoji: '🖐️✨',
    description: (
      <>
        You can send and receive OSC just by adding an Attribute to a class or function.
      </>
    ),
  },
  {
    title: 'Easily Combinable',
    emoji: '🧩',
    description: (
      <>
        It can be combined without being affected by other plugins or settings.
      </>
    ),
  },
];

function Feature({title, emoji, description}: FeatureItem) {
  return (
    <div className={clsx('col col--4')}>
      <div className={`${styles.featureEmoji} text--center`}>
        {emoji}
      </div>
      <div className="text--center padding-horiz--md">
        <Heading as="h3">{title}</Heading>
        <p>{description}</p>
      </div>
    </div>
  );
}

export default function HomepageFeatures(): ReactNode {
  return (
    <section className={styles.features}>
      <div className="container">
        <div className="row">
          {FeatureList.map((props, idx) => (
            <Feature key={idx} {...props} />
          ))}
        </div>
      </div>
    </section>
  );
}
